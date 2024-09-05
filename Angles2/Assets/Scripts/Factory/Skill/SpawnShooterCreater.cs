using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnShooterData : BaseSkillData
{
    public BaseWeapon.Name _shooterType;
    public List<ITarget.Type> _targetTypes;
    public ShooterData _data;

    public SpawnShooterData(int maxUpgradePoint, BaseWeapon.Name shooterType, ShooterData data, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _shooterType = shooterType;
        _targetTypes = targetTypes;
        _data = data;
    }
}


public class SpawnShooterCreater : SkillCreater
{
    BaseFactory _weaponFactory;
    public ShooterData _data;

    public SpawnShooterCreater(BaseSkillData data, BaseFactory _weaponFactory) : base(data)
    {
        this._weaponFactory = _weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnShooterData data = _buffData as SpawnShooterData;
        return new SpawnShooter(data, _weaponFactory);
    }
}