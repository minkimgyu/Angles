using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

[Serializable]
public class SpreadMultipleBulletsData : SkillData
{
    [JsonProperty] private float _waveDelay;
    [JsonProperty] private int _maxWaveCount;

    [JsonProperty] private float _damage;
    [JsonProperty] private float _force;
    [JsonProperty] private float _delay;

    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;

    [JsonProperty] private BaseWeapon.Name _bulletName;
    [JsonProperty] private float _bulletCount;
    [JsonProperty] private float _distanceFromCaster;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float Force { get => _force; set => _force = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }

    [JsonIgnore] public float WaveDelay { get => _waveDelay; set => _waveDelay = value; }
    [JsonIgnore] public int MaxWaveCount { get => _maxWaveCount; set => _maxWaveCount = value; }

    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float BulletCount { get => _bulletCount; set => _bulletCount = value; }
    [JsonIgnore] public float DistanceFromCaster { get => _distanceFromCaster; set => _distanceFromCaster = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }
    [JsonIgnore] public BaseWeapon.Name BulletName { get => _bulletName; set => _bulletName = value; }

    public SpreadMultipleBulletsData(
        int maxUpgradePoint,
        float waveDelay,
        int maxWaveCount,

        float damage,
        float adRatio,
        float groggyDuration,

        float delay,
        float force,
        BaseWeapon.Name bulletName,
        float bulletCount,
        float distanceFromCaster,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _waveDelay = waveDelay;
        _maxWaveCount = maxWaveCount;

        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;

        _delay = delay;
        _force = force;

        _bulletName = bulletName;
        _bulletCount = bulletCount;
        _distanceFromCaster = distanceFromCaster;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpreadMultipleBulletsData(
            _maxUpgradePoint, // SkillData���� ��ӵ� ��
            _waveDelay,
            _maxWaveCount,
            _damage,
            _adRatio,
            _groggyDuration,
            _delay,
            _force,
            _bulletName,
            _bulletCount,
            _distanceFromCaster,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ ���� ����
        );
    }
}

public class SpreadMultipleBulletsCreater : SkillCreater
{
    BaseFactory _weaponFactory;

    public SpreadMultipleBulletsCreater(SkillData data, BaseFactory weaponFactory) : base(data)
    {
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpreadMultipleBulletsData data = CopySkillData as SpreadMultipleBulletsData;
        return new SpreadMultipleBullets(data, _weaponFactory);
    }
}
