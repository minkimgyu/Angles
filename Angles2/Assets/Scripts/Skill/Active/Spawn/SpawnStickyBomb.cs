using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnStickyBomb : CooltimeSkill
{
    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;
    StickyBombData _data;

    public SpawnStickyBomb(SpawnStickyBombData data, BaseFactory weaponFactory) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _data = data._data;
        _targetTypes = data._targetTypes;
        _weaponFactory = weaponFactory;
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

        if (_stackCount <= 0) return;
        _stackCount--;

        weapon.ResetData(_data);
        weapon.Upgrade(UpgradePoint);
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetFollower(followable);
    }
}
