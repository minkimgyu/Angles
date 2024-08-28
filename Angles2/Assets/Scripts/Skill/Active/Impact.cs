using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Impact : RandomSkill
{
    public List<ITarget.Type> _targetTypes;
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    List<ImpactUpgradableData> _upgradableDatas;
    ImpactUpgradableData CurrentUpgradableData { get { return _upgradableDatas[_upgradePoint]; } }

    public Impact(ImpactData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data._maxUpgradePoint, data._probability)
    {
        _upgradableDatas = data._upgradableDatas;
        _targetTypes = data._targetTypes;

        this.CreateEffect = CreateEffect;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log("Impact");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = CreateEffect?.Invoke(BaseEffect.Name.ImpactEffect);
        if (effect == null) return;

        effect.ResetPosition(contactPos);
        effect.ResetSize(CurrentUpgradableData.Range);
        effect.Play();

        DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes);
        Damage.HitCircleRange(damageData, contactPos, CurrentUpgradableData.Range, true, Color.red, 3);
    }
}
