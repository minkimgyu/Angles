using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnStickyBomb : BaseSkill
{
    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;
    StickyBombData _data;
    SpawnStickyBombUpgrader _upgrader;

    public SpawnStickyBomb(SpawnStickyBombData data, SpawnStickyBombUpgrader upgrader, BaseFactory weaponFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _upgrader = upgrader;
        _data = data._data;
        _targetTypes = data._targetTypes;
        _weaponFactory = weaponFactory;
        _useConstraint = new CooltimeConstraint(data.maxStackCount, data._coolTime);
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

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        IFollowable followable = collision.gameObject.GetComponent<IFollowable>();
        if (followable == null) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.StickyBomb);
        if (weapon == null) return;

        if (_useConstraint.CanUse() == false) return;
        _useConstraint.Use();

        weapon.ResetData(_data);
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetFollower(followable);
    }
}
