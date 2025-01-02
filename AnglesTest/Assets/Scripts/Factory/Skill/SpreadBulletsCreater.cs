using DamageUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

[Serializable]
public class SpreadBulletsData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;

    [JsonProperty] private BaseWeapon.Name _bulletName;

    [JsonProperty] private float _delay;
    [JsonProperty] private float _force;
    [JsonProperty] private float _bulletCount;
    [JsonProperty] private float _distanceFromCaster;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public float Force { get => _force; set => _force = value; }
    [JsonIgnore] public float BulletCount { get => _bulletCount; set => _bulletCount = value; }
    [JsonIgnore] public float DistanceFromCaster { get => _distanceFromCaster; set => _distanceFromCaster = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }
    [JsonIgnore] public BaseWeapon.Name BulletName { get => _bulletName; set => _bulletName = value; }

    public SpreadBulletsData(
        int maxUpgradePoint,
        float damage,
        float adRatio,

        BaseWeapon.Name bulletName,

        float delay,
        float force,
        float bulletCount,
        float distanceFromCaster,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = 0;

        _bulletName = bulletName;

        _delay = delay;
        _force = force;
        _bulletCount = bulletCount;
        _distanceFromCaster = distanceFromCaster;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpreadBulletsData(
            _maxUpgradePoint,
            _damage,
            _adRatio,
            _bulletName,
            _delay,
            _force,
            _bulletCount,
            _distanceFromCaster,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SpreadBulletsCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;

    public SpreadBulletsCreater(SkillData data, BaseFactory weaponFactory) : base(data)
    {
        _weaponFactory = weaponFactory;
    }

    public SpreadBulletsCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpreadBulletsData data = CopySkillData as SpreadBulletsData;
        return new SpreadBullets(data, _upgrader, _weaponFactory);
    }
}
