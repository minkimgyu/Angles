using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public class SpawnRocketShooter : BaseSkill
//{
//    Shooter _shooter;
//    BaseFactory _weaponFactory;

//    ShooterData _shooterData;
//    RocketData _rocketData;

//    public SpawnRocketShooter(SpawnShooterData data, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
//    {
//        _targetTypes = data._targetTypes;
//        _weaponFactory = weaponFactory;
//    }

//    public override void Upgrade()
//    {
//        base.Upgrade();

//        _upgradeVisitor.Visit(this, )
//        _shooter.Upgrade();
//    }

//    public override void OnAdd()
//    {
//        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.RocketShooter);
//        if (weapon == null) return;

//        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
//        if (followable == null) return;

//        weapon.ResetFollower(followable);

//        weapon.ResetData(_shooterData, _rocketData);
//        weapon.ResetTargetTypes(_targetTypes);
//        weapon.ResetPosition(_castingData.MyTransform.position);
//        _shooter = weapon;
//    }
//}