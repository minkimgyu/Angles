using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Impact : BaseSkill
{
    ImpactData _data;
    BaseFactory _effectFactory;

    public Impact(ImpactData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override void OnAdd()
    {
        _useConstraintStrategy = new RandomConstraintStrategy(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override bool OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return false;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return false;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Impact, 0.7f);
        Debug.Log("Impact");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ImpactEffect);
        if (effect == null) return false;

        effect.ResetPosition(contactPos);
        effect.ResetSize(_data.RangeMultiplier);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        Damage.HitCircleRange(damageData, contactPos, _data.Range * _data.RangeMultiplier, true, Color.red, 3);
        return true;
    }
}
