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
    [JsonConverter(typeof(StringEnumConverter))] private BaseWeapon.Name _shooterName;
    [JsonConverter(typeof(StringEnumConverter))] private BaseWeapon.Name _projectileName;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public BaseWeapon.Name ShooterName { get => _shooterName; set => _shooterName = value; }
    [JsonIgnore] public BaseWeapon.Name ProjectileName { get => _projectileName; set => _projectileName = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public SpawnShooterData(
        int maxUpgradePoint,
        BaseWeapon.Name shooterName,
        float damage,
        float adRatio,
        float groggyDuration,
        float delay,
        BaseWeapon.Name projectileName,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _shooterName = shooterName;
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _delay = delay;
        _projectileName = projectileName;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnShooterData(
            _maxUpgradePoint, // SkillData���� ��ӵ� ��
            _shooterName,
            _damage,
            _adRatio,
            _groggyDuration,
            _delay,
            _projectileName,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ ���� ����
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