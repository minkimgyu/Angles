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
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;
    public ShooterData _data;

    public SpawnShooterCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnShooterData data = _skillData as SpawnShooterData;
        return new SpawnShooter(data, CreateWeapon);
    }
}