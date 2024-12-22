using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// ���׷��̵� ������ ��ų �����͸� ���� Struct�� ���� ��������
[Serializable]
public class StatikkData : CooltimeSkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _range;
    [JsonProperty] private int _maxTargetCount;
    [JsonProperty] private float _groggyDuration;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public int MaxTargetCount { get => _maxTargetCount; set => _maxTargetCount = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public StatikkData(
        int maxUpgradePoint,
        float coolTime,
        int maxStackCount,
        float damage,
        float adRatio,
        float range,
        int maxTargetCount,
        float groggyDuration,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _adRatio = adRatio;
        _range = range;
        _maxTargetCount = maxTargetCount;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new StatikkData(
            _maxUpgradePoint, // CooltimeSkillData���� ��ӵ� ��
            _coolTime, // CooltimeSkillData���� ��ӵ� ��
            _maxStackCount, // CooltimeSkillData���� ��ӵ� ��
            _damage,
            _adRatio,
            _range,
            _maxTargetCount,
            _groggyDuration,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ ���� ����
        );
    }
}

public class StatikkCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public StatikkCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data) 
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        StatikkData data = CopySkillData as StatikkData;
        return new Statikk(data, _upgrader, _effectFactory);
    }
}
