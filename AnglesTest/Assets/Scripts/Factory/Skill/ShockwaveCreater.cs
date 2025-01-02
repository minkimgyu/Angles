using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class ShockwaveData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _delay;
    [JsonProperty] private float _range;
    [JsonProperty] private float _sizeMultiplier;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; set => _sizeMultiplier = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public ShockwaveData(int maxUpgradePoint, float damage, float adRatio, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _delay = delay;
        _range = range;
        _sizeMultiplier = 1;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ShockwaveData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _damage,
            _adRatio,
            _range,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class ShockwaveCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public ShockwaveCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ShockwaveData data = CopySkillData as ShockwaveData;
        return new Shockwave(data, _upgrader, _effectFactory);
    }
}