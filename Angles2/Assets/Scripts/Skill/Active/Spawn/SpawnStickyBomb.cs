using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class SpawnStickyBomb : CooltimeSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnStickyBomb(SpawnStickyBombData data) : base(data._maxUpgradePoint, data._coolTime, data._maxStackCount)
    {
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        IFollowable followable = collision.gameObject.GetComponent<IFollowable>();
        if (followable == null) return;

        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.StickyBomb);
        if (weapon == null) return;

        if (_stackCount <= 0) return;
        _stackCount--;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetFollower(followable);
    }
}
