using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnShooterData : SkillData
{
    public BaseWeapon.Name _shooterType;
    public List<ITarget.Type> _targetTypes;
    public ShooterData _shooterData;

    public SpawnShooterData(int maxUpgradePoint, BaseWeapon.Name shooterType, ShooterData shooterData, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _shooterType = shooterType;
        _targetTypes = targetTypes;
        _shooterData = shooterData;
    }
}


public class SpawnShooterCreater : SkillCreater
{
    BaseFactory _weaponFactory;
    public ShooterData _data;

    public SpawnShooterCreater(SkillData data, BaseFactory _weaponFactory) : base(data)
    {
        this._weaponFactory = _weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnShooterData data = _skillData as SpawnShooterData;
        return new SpawnShooter(data, _weaponFactory);
    }
}