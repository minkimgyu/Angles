using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class SpawnShooterData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;
    [JsonProperty] private float _delay;
    [JsonProperty] [JsonConverter(typeof(StringEnumConverter))] private BaseWeapon.Name _shooterName;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public BaseWeapon.Name ShooterName { get => _shooterName; set => _shooterName = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public SpawnShooterData(
        int maxUpgradePoint,
        BaseWeapon.Name shooterName,
        float damage,
        float adRatio,
        float groggyDuration,
        float delay,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _shooterName = shooterName;
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _delay = delay;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnShooterData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _shooterName,
            _damage,
            _adRatio,
            _groggyDuration,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}


public class SpawnShooterCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;
    ShooterData _data;

    public SpawnShooterCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnShooterData data = CopySkillData as SpawnShooterData;
        return new SpawnShooter(data, _upgrader, _weaponFactory);
    }
}