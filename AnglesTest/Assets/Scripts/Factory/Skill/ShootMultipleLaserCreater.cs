using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShootMultipleLaserData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;

    [JsonProperty] private float _maxDistance;
    [JsonProperty] private float _delay;

    [JsonProperty] private int _totalLaserCount;
    [JsonProperty] private int _shootableLaserCount;

    [JsonProperty] private float _distanceFromCaster;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }

    [JsonIgnore] public int TotalLaserCount { get => _totalLaserCount; set => _totalLaserCount = value; }
    [JsonIgnore] public int ShootableLaserCount { get => _shootableLaserCount; set => _shootableLaserCount = value; }

    [JsonIgnore] public float DistanceFromCaster { get => _distanceFromCaster; set => _distanceFromCaster = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }
    [JsonIgnore] public float MaxDistance { get => _maxDistance; set => _maxDistance = value; }

    public ShootMultipleLaserData(
        int maxUpgradePoint,
        float damage,
        float adRatio,

        float maxDistance,
        float delay,

        int totalLaserCount,
        int shootableLaserCount,

        float distanceFromCaster,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = 0;

        _maxDistance = maxDistance;
        _delay = delay;

        _totalLaserCount = totalLaserCount;
        _shootableLaserCount = shootableLaserCount;

        _distanceFromCaster = distanceFromCaster;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ShootMultipleLaserData(
            _maxUpgradePoint,
            _damage,
            _adRatio,
            _maxDistance,
            _delay,

            _totalLaserCount,
            _shootableLaserCount,

            _distanceFromCaster,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class ShootMultipleLaserCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ShootMultipleLaserCreater(SkillData data, BaseFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ShootMultipleLaserData data = CopySkillData as ShootMultipleLaserData;
        return new ShootMultipleLaser(data, _effectFactory);
    }
}
