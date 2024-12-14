using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Knockback : BaseSkill
{
    BaseFactory _effectFactory;
    KnockbackData _data;

    public Knockback(KnockbackData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data._maxUpgradePoint)
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

    public override void OnReflect(Collision2D collision) 
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        Transform casterTransform = _caster.GetComponent<Transform>();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Knockback);
        Debug.Log("Knockback");

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.KnockbackEffect);
        effect.ResetSize(_data._rangeMultiplier);
        effect.ResetPosition(casterTransform.position, casterTransform.right);
        effect.Play();

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data._damage,
                _upgradeableRatio.AttackDamage,
                _data._adRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data._targetTypes,
            _data._groggyDuration
        );


        Damage.HitBoxRange(damageData, casterTransform.position, _data._offset.V2, casterTransform.right,
            _data._size.V2 * _data._rangeMultiplier, true, Color.red);
    }
}
