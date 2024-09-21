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
            _maxUpgradePoint, // RandomSkillData���� ��ӵ� ��
            _probability, // RandomSkillData���� ��ӵ� ��
            _damage,
            _range,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ�� ���� ����
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
