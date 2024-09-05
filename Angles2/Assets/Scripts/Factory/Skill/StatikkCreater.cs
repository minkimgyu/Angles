using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// ���׷��̵� ������ ��ų �����͸� ���� Struct�� ���� ��������

public struct StatikkUpgradableData
{
    public StatikkUpgradableData(float damage, float range, int maxTargetCount)
    {
        _damage = damage;
        _range = range;
        _maxTargetCount = maxTargetCount;
    }

    private float _damage;
    private float _range;
    private int _maxTargetCount;

    public float Damage { get => _damage; }
    public float Range { get => _range; }
    public int MaxTargetCount { get => _maxTargetCount; }
}

[Serializable]
public class StatikkData : CooltimeSkillData
{
    public List<StatikkUpgradableData> _upgradableDatas;
    public List<ITarget.Type> _targetTypes;

    public StatikkData(int maxUpgradePoint, float coolTime, int maxStackCount, List<StatikkUpgradableData> upgradableDatas, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _upgradableDatas = upgradableDatas;
        _targetTypes = targetTypes;
    }
}

public class StatikkCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public StatikkCreater(BaseSkillData data, BaseFactory effectFactory) : base(data) 
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        StatikkData data = _buffData as StatikkData;
        return new Statikk(data, _effectFactory);
    }
}
