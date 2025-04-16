using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Skill;

[Serializable]
public class SpawnBlackholeData : RandomSkillData
{
    [JsonProperty] private float _lifeTime;
    [JsonProperty] private float _sizeMultiplier;

    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Lifetime { get => _lifeTime; set => _lifeTime = value; }
    [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; set => _sizeMultiplier = value; }

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }

    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public SpawnBlackholeData(
        int maxUpgradePoint,
        float probability,

        float damage,
        float adRatio,
        float groggyDuration,

        float lifetime,
        float sizeMultiplier,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _lifeTime = lifetime;
        _sizeMultiplier = sizeMultiplier;

        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;

        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnBlackholeData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값

            _damage,
            _adRatio,
            _groggyDuration,

            _lifeTime,
            _sizeMultiplier,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SpawnBlackholeCreater : SkillCreater
{
    BaseFactory _weaponFactory;
    IUpgradeVisitor _upgrader;

    public SpawnBlackholeCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnBlackholeData data = CopySkillData as SpawnBlackholeData;
        return new SpawnBlackhole(data, _upgrader, _weaponFactory);
    }
}
