using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnRocketShooter : BaseSkill
{
    BaseWeapon _shooter;

    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;
    ShooterData _data;

    public SpawnRocketShooter(SpawnShooterData data, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
    {
        _targetTypes = data._targetTypes;
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade(int step)
    {
        base.Upgrade(step);

        if (_shooter == null) return;
        _shooter.Upgrade(step);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.RocketShooter);
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