using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Skill;

[Serializable]
public class SelfDestructionData : SkillData
{
    [JsonProperty] private float _delay;
    [JsonProperty] private float _hpRatioOnInvoke;
    [JsonProperty] private float _damage;
    [JsonProperty] private float _rangeMultiplier;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _range;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float RangeMultiplier { get => _rangeMultiplier; set => _rangeMultiplier = value; }

    [JsonIgnore] public float HpRatioOnInvoke { get => _hpRatioOnInvoke; set => _hpRatioOnInvoke = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public SelfDestructionData(
        int maxUpgradePoint,
        float damage,
        float adRatio,
        float range,
        float rangeMultiplier,
        float delay,
        float hpRatioOnInvoke,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _delay = delay;
        _range = range;
        _rangeMultiplier = rangeMultiplier;

        _hpRatioOnInvoke = hpRatioOnInvoke;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SelfDestructionData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _damage,
            _adRatio,
            _range,
            _rangeMultiplier,
            _delay,
            _hpRatioOnInvoke,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SelfDestructionCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public SelfDestructionCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        SelfDestructionData data = CopySkillData as SelfDestructionData;
        return new SelfDestruction(data, _upgrader, _effectFactory);
    }
}