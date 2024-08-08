using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnStickyBomb : CooltimeSkill
{
    List<ITarget.Type> _targetTypes;
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnStickyBomb(SpawnStickyBombData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _targetTypes = data._targetTypes;
        this.CreateWeapon = CreateWeapon;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        IFollowable followable = collision.gameObject.GetComponent<IFollowable>();
        if (followable == null) return;

        BaseWeapon weapon = CreateWeapon?.Invoke(BaseWeapon.Name.StickyBomb);
        if (weapon == null) return;

        if (_stackCount <= 0) return;
        _stackCount--;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetFollower(followable);
    }
}
