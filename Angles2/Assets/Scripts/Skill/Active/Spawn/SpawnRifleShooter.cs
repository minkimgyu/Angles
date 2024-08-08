using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnRifleShooter : BaseSkill
{
    List<ITarget.Type> _targetTypes;

    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnRifleShooter(SpawnRifleShooterData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(Type.Passive, data._maxUpgradePoint)
    {
        _targetTypes = data._targetTypes;
        this.CreateWeapon = CreateWeapon;
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = CreateWeapon?.Invoke(BaseWeapon.Name.RifleShooter);
        if (weapon == null) return;

        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
        if (followable == null) return;

        weapon.ResetFollower(followable);

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}