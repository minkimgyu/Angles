using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnShooter : BaseSkill
{
    BaseWeapon _shooter;

    List<ITarget.Type> _targetTypes;
    BaseFactory _weaponFactory;
    BaseWeapon.Name _shooterType;

    SpawnShooterData _data;
    ShooterData _shooterData;

    // 여기서 슈터 데이터와 무기 데이터를 같이 받아오기
    public SpawnShooter(SpawnShooterData data, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
    {
        _data = data;
        _shooterData = data._shooterData;
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgradeVisitor.Visit(this, _shooterData);
    }


    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(_shooterType);
        if (weapon == null) return;

        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
        if (followable == null) return;

        weapon.ResetFollower(followable);
        weapon.ResetData(_shooterData);
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
        _shooter = weapon;
    }
}