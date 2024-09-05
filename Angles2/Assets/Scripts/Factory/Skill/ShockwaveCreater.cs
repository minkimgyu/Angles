using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShockwaveData : BaseSkillData
{
    public float _damage;
    public float _delay;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ShockwaveData(int maxUpgradePoint, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _delay = delay;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class ShockwaveCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ShockwaveCreater(BaseSkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ShockwaveData data = _buffData as ShockwaveData;
        return new Shockwave(data, _effectFactory);
    }
}