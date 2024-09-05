using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnShooter : BaseSkill
{
    BaseWeapon _shooter;

    List<ITarget.Type> _targetTypes;

    BaseFactory _weaponFactory;
    ShooterData _data;
    BaseWeapon.Name _shooterType;

    public SpawnShooter(SpawnShooterData data, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
    {
        _shooterType = data._shooterType;
        _data = data._data;
        _targetTypes = data._targetTypes;
        _weaponFactory = weaponFactory;
    }

    protected override void OnUpgradeRequested()
    {
        if (_shooter == null) return;
        _shooter.Upgrade(UpgradePoint);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(_shooterType);
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