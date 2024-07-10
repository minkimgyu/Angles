using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatikkData : BaseSkillData
{
    public float _damage;
    public float _range;
    public int _maxTargetCount;
    public List<ITarget.Type> _targetTypes;

    public StatikkData(float probability, float damage, float range, int maxTargetCount, List<ITarget.Type> targetTypes) : base(probability)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
        _maxTargetCount = maxTargetCount;
    }
}

public class StatikkCreater : SkillCreater<StatikkData>
{
    public override BaseSkill Create()
    {
        return new Statikk(_data);
    }
}
