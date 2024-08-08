using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnBlackholeData : RandomSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnBlackholeData(int maxUpgradePoint, float probability, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnBlackholeCreater : SkillCreater
{
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnBlackholeCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnBlackholeData data = _skillData as SpawnBlackholeData;
        return new SpawnBlackhole(data, CreateWeapon);
    }
}
