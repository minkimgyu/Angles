using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactData : BaseSkillData
{
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ImpactData(float probability, float damage, float range, List<ITarget.Type> targetTypes) : base(probability)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class ImpactCreater : SkillCreater<ImpactData>
{
    public override BaseSkill Create()
    {
        return new Impact(_data);
    }
}
