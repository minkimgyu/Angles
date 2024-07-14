using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShockwaveData : BaseSkillData
{
    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ShockwaveData(float probability, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(probability)
    {
        _delay = delay;
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class ShockwaveCreater : SkillCreater<ShockwaveData>
{
    public override BaseSkill Create()
    {
        return new Shockwave(_data);
    }
}