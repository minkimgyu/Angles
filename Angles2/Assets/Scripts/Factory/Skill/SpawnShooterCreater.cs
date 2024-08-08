using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnRifleShooterData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnRifleShooterData(int maxUpgradePoint, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _targetTypes = targetTypes;
    }
}


public class SpawnRifleShooterCreater : SkillCreater
{
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnRifleShooterCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnRifleShooterData data = _skillData as SpawnRifleShooterData;
        return new SpawnRifleShooter(data, CreateWeapon);
    }
}

[Serializable]
public class SpawnRocketShooterData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnRocketShooterData(int maxUpgradePoint, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnRocketShooterCreater : SkillCreater
{
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnRocketShooterCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnRocketShooterData data = _skillData as SpawnRocketShooterData;
        return new SpawnRocketShooter(data, CreateWeapon);
    }
}
