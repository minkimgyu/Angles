using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnShooter : BaseSkill
{
    BaseWeapon _shooter;

    List<ITarget.Type> _targetTypes;

    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;
    ShooterData _data;
    BaseWeapon.Name _shooterType;

    public SpawnShooter(SpawnShooterData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(Type.Passive, data._maxUpgradePoint)
    {
        _shooterType = data._shooterType;
        _data = data._data;
        _targetTypes = data._targetTypes;
        this.CreateWeapon = CreateWeapon;
    }

    public override void Upgrade(int step)
    {
        base.Upgrade(step);

        if (_shooter == null) return;
        _shooter.Upgrade(step);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = CreateWeapon?.Invoke(_shooterType);
        if (weapon == null) return;

        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
        if (followable == null) return;

        weapon.ResetFollower(followable);

        weapon.ResetData(_data);
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
        _shooter = weapon;
    }
}