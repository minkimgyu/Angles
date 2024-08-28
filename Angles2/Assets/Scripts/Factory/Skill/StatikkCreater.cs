using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// 업그레이드 가능한 스킬 데이터를 따로 Struct로 빼서 관리하자

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
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    public StatikkCreater(BaseSkillData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data) 
    {
        this.CreateEffect = CreateEffect;
    }

    public override BaseSkill Create()
    {
        StatikkData data = _skillData as StatikkData;
        return new Statikk(data, CreateEffect);
    }
}
