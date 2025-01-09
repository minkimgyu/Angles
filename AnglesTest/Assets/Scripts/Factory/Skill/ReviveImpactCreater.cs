using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReviveImpactData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _rangeMultiplier;
    [JsonProperty] private float _range;
    [JsonProperty] private float _groggyDuration;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float RangeMultiplier { get => _rangeMultiplier; set => _rangeMultiplier = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public ReviveImpactData(
        int maxUpgradePoint,
        float adRatio,
        float range,
        float groggyDuration,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = DamageUtility.Damage.InstantDeathDamage;
        _adRatio = adRatio;

        _range = range;
        _rangeMultiplier = 1;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ReviveImpactData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _adRatio,
            _range,
            _groggyDuration,
            new List<ITarget.Type>(_targetTypes) // 리스트의 깊은 복사
        );
    }
}

public class ReviveImpactCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ReviveImpactCreater(SkillData data, BaseFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ReviveImpactData data = CopySkillData as ReviveImpactData;
        return new ReviveImpact(data, _effectFactory);
    }
}
