using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBladeData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnBladeData(float probability, List<ITarget.Type> targetTypes) : base(probability)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnBladeCreater : SkillCreater<SpawnBladeData>
{
    public override BaseSkill Create()
    {
        return new SpawnBlade(_data);
    }
}
