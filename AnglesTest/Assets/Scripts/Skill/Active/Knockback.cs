using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Knockback : BaseSkill
{
    BaseFactory _effectFactory;
    KnockbackData _data;

    public Knockback(KnockbackData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
        _upgrader = upgrader;
    }

    public override void OnAdd()
    {
        _useConstraint = new CooltimeConstraint(_data, _upgradeableRatio);
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

        Transform casterTransform = _caster.GetComponent<Transform>();

        IForce forceTarget = targetObject.GetComponent<IForce>();
        if (forceTarget != null) forceTarget.ApplyForce(casterTransform.forward, _data.Force, ForceMode2D.Impulse);

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Knockback);
        Debug.Log("Knockback");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetSize(_data.RangeMultiplier);
        effect.ResetPosition(casterTransform.position, casterTransform.right);
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


        Damage.HitBoxRange(damageData, casterTransform.position, _data.Offset.V2, casterTransform.right,
            _data.Size.V2 * _data.RangeMultiplier, true, Color.red);
        return true;
    }
}
