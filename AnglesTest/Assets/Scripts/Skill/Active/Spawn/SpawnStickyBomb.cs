using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnStickyBomb : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnStickyBombData _data;

    public SpawnStickyBomb(SpawnStickyBombData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _upgrader = upgrader;
        _data = data;
        _weaponFactory = weaponFactory;
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

        IFollowable followable = collision.gameObject.GetComponent<IFollowable>();
        if (followable == null) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.StickyBomb);
        if (weapon == null) return;

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

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data._delay));

        weapon.ModifyData(modifiers);
        weapon.Activate();
        weapon.ResetFollower(followable);
    }
}
