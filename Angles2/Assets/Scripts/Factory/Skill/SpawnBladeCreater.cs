using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnBladeData : RandomSkillData
{
    public List<ITarget.Type> _targetTypes;
    public float _force;

    public SpawnBladeData(int maxUpgradePoint, float probability, float force, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _targetTypes = targetTypes;
        _force = force;
    }
}

public class SpawnBladeCreater : SkillCreater
{
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnBladeCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnBladeData data = _skillData as SpawnBladeData;
        return new SpawnBlade(data, CreateWeapon);
    }
}
