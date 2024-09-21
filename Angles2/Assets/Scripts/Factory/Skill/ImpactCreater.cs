using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ImpactData : RandomSkillData
{
    public float _damage;
    public float _rangeMultiplier;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ImpactData(
        int maxUpgradePoint,
        float probability,
        float damage,
        float range,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _range = range;
        _rangeMultiplier = 1;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ImpactData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값
            _damage,
            _range,
            new List<ITarget.Type>(_targetTypes) // 리스트의 깊은 복사
        );
    }
}

public class ImpactCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public ImpactCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ImpactData data = _skillData as ImpactData;
        return new Impact(data, _upgrader, _effectFactory);
    }
}
