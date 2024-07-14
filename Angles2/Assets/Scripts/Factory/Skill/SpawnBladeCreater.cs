using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBladeData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;
    public float _force;

    public SpawnBladeData(float probability, float force, List<ITarget.Type> targetTypes) : base(probability)
    {
        _targetTypes = targetTypes;
        _force = force;
    }
}

public class SpawnBladeCreater : SkillCreater<SpawnBladeData>
{
    public override BaseSkill Create()
    {
        return new SpawnBlade(_data);
    }
}
