using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class SpawnBladeData : RandomSkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;

    [JsonProperty] private float _lifetime;
    [JsonProperty] private float _sizeMultiplier;

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;
    [JsonProperty] private float _force;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float Lifetime { get => _lifetime; set => _lifetime = value; }
    [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; set => _sizeMultiplier = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }
    [JsonIgnore] public float Force { get => _force; set => _force = value; }

    public SpawnBladeData(int maxUpgradePoint, float probability, float damage, float adRatio, float groggyDuration, float lifetime, float force, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _lifetime = lifetime;
        _sizeMultiplier = 1;

        _force = force;
        _targetTypes = targetTypes;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
    }

    public override SkillData Copy()
    {
        return new SpawnBladeData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값
            _damage,
            _adRatio,
            _groggyDuration,
            _lifetime,
            _force,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SpawnBladeCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;

    public SpawnBladeCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnBladeData data = CopySkillData as SpawnBladeData;
        return new SpawnBlade(data, _upgrader, _weaponFactory);
    }
}
