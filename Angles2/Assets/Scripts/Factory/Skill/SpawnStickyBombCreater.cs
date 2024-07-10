using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnStickyBombData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnStickyBombData(float probability, List<ITarget.Type> targetTypes) : base(probability)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnStickyBombCreater : SkillCreater<SpawnStickyBombData>
{
    public override BaseSkill Create()
    {
        return new SpawnStickyBomb(_data);
    }
}
