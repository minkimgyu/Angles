using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnBlackholeData : RandomSkillData
{
    public List<ITarget.Type> _targetTypes;
    public BlackholeData _data;

    public SpawnBlackholeData(int maxUpgradePoint, float probability, BlackholeData data, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _targetTypes = targetTypes;
        _data = data;
    }
}

public class SpawnBlackholeCreater : SkillCreater
{
    BaseFactory _weaponFactory;

    public SpawnBlackholeCreater(BaseSkillData data, BaseFactory weaponFactory) : base(data)
    {
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnBlackholeData data = _buffData as SpawnBlackholeData;
        return new SpawnBlackhole(data, _weaponFactory);
    }
}
