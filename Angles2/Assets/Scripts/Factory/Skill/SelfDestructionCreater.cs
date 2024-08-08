using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SelfDestructionData : BaseSkillData
{
    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public SelfDestructionData(int maxUpgradePoint, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _delay = delay;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class SelfDestructionCreater : SkillCreater
{
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    public SelfDestructionCreater(BaseSkillData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data)
    {
        this.CreateEffect = CreateEffect;
    }

    public override BaseSkill Create()
    {
        SelfDestructionData data = _skillData as SelfDestructionData;
        return new SelfDestruction(data, CreateEffect);
    }
}