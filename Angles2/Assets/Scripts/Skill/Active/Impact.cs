using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Impact : BaseSkill
{
    public List<ITarget.Type> _targetTypes;
    BaseFactory _effectFactory;

    List<ImpactUpgradableData> _upgradableDatas;
    ImpactUpgradableData CurrentUpgradableData { get { return _upgradableDatas[UpgradePoint]; } }

    public Impact(ImpactData data,  BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _upgradableDatas = data._upgradableDatas;
        _targetTypes = data._targetTypes;

        _effectFactory = effectFactory;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log("Impact");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return;

        effect.ResetPosition(contactPos);
        effect.ResetSize(CurrentUpgradableData.Range);
        effect.Play();

        DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes);
        Damage.HitCircleRange(damageData, contactPos, CurrentUpgradableData.Range, true, Color.red, 3);
    }
}
