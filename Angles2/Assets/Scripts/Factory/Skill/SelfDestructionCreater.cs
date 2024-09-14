using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SelfDestructionData : SkillData
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
    BaseFactory _effectFactory;

    public SelfDestructionCreater(SkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        SelfDestructionData data = _skillData as SelfDestructionData;
        return new SelfDestruction(data, _effectFactory);
    }
}