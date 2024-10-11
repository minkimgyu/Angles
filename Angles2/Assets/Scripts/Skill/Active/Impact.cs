using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Impact : BaseSkill
{
    ImpactData _data;
    BaseFactory _effectFactory;

    public Impact(ImpactData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new RandomConstraintComponent(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Impact, 0.7f);
        Debug.Log("Impact");

        Vector3 contactPos = collision.contacts[0].point;
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return;

        effect.ResetPosition(contactPos);
        effect.ResetSize(_data._rangeMultiplier);
        effect.Play();

        DamageableData damageData = 
        new DamageableData.DamageableDataBuilder().
        SetDamage(new DamageData(_data._damage, _upgradeableRatio.TotalDamageRatio))
        .SetTargets(_data._targetTypes)
        .SetGroggyDuration(_data._groggyDuration)
        .Build();

        Damage.HitCircleRange(damageData, contactPos, _data._range * _data._rangeMultiplier, true, Color.red, 3);
    }
}
