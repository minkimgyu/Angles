using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelfDestructionData : BaseSkillData
{
    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public SelfDestructionData(float probability, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(probability)
    {
        _delay = delay;
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class SelfDestructionCreater : SkillCreater<SelfDestructionData>
{
    public override BaseSkill Create()
    {
        return new SelfDestruction(_data);
    }
}