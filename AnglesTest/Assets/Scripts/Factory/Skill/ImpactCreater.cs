using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Skill;

[System.Serializable]
public class ImpactData : RandomSkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _rangeMultiplier;

    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _range;
    [JsonProperty] private float _groggyDuration;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float RangeMultiplier { get => _rangeMultiplier; set => _rangeMultiplier = value; }

    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public ImpactData(
        int maxUpgradePoint,
        float probability,
        float damage,
        float rangeMultiplier,
        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _adRatio = adRatio;

        _range = range;
        _rangeMultiplier = rangeMultiplier;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ImpactData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값
            _damage,
            _rangeMultiplier,
            _adRatio,
            _range,
            _groggyDuration,
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
        ImpactData data = CopySkillData as ImpactData;
        return new Impact(data, _upgrader, _effectFactory);
    }
}
