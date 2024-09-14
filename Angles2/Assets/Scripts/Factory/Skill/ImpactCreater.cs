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
    public float _damage;
    public float _range;

    public List<ImpactUpgradableData> _upgradableDatas;
    public List<ITarget.Type> _targetTypes;

    // + 연산자 오버로딩
    public static ImpactData operator +(ImpactData a, ImpactUpgrader.UpgradableData b)
    {
        return new ImpactData(
            a._maxUpgradePoint,
            a._probability,
            a._damage + b.Damage,
            a._range + b.Range,
            a._targetTypes
        );
    }

    public ImpactData(int maxUpgradePoint, float probability, float damage, float range, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class ImpactCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ImpactCreater(SkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ImpactData data = _skillData as ImpactData;
        return new Impact(data, _effectFactory);
    }
}
