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

        if (_useConstraint.CanUse() == false) return;
        _useConstraint.Use();

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(_data._damage));
        modifiers.Add(new WeaponDelayModifier(_data._delay));
        modifiers.Add(new WeaponTargetModifier(_data._targetTypes));

        weapon.ModifyData(modifiers);
        weapon.ResetFollower(followable);
    }
}
