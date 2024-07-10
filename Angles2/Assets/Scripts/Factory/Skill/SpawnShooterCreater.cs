using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnShooterData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnShooterData(float probability, List<ITarget.Type> targetTypes) : base(probability)
    {
        _targetTypes = targetTypes;
    }
}


public class SpawnShooterCreater : SkillCreater<SpawnShooterData>
{
    public override BaseSkill Create()
    {
        return new SpawnShooter(_data);
    }
}
