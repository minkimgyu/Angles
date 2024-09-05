using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct ImpactUpgradableData
{
    public ImpactUpgradableData(float damage, float range)
    {
        _damage = damage;
        _range = range;
    }

    private float _damage;
    private float _range;

    public float Damage { get => _damage; }
    public float Range { get => _range; }
}


[System.Serializable]
public class ImpactData : RandomSkillData
{
    public List<ImpactUpgradableData> _upgradableDatas;
    public List<ITarget.Type> _targetTypes;

    public ImpactData(int maxUpgradePoint, float probability, List<ImpactUpgradableData> upgradableDatas, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _upgradableDatas = upgradableDatas;
        _targetTypes = targetTypes;
    }
}

public class ImpactCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ImpactCreater(BaseSkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ImpactData data = _buffData as ImpactData;
        return new Impact(data, _effectFactory);
    }
}
