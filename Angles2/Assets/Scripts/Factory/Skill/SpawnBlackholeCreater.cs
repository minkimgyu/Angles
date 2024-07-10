using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBlackholeData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnBlackholeData(float probability, List<ITarget.Type> targetTypes) : base(probability)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnBlackholeCreater : SkillCreater<SpawnBlackholeData>
{
    public override BaseSkill Create()
    {
        return new SpawnBlackhole(_data);
    }
}
