using DamageUtility;
using System;
using System.Collections.Generic;

[Serializable]
public class SpreadBulletsData : SkillData
{
    public float _damage;
    public float _adRatio;
    public float _groggyDuration;

    public float _delay;
    public float _force;
    public float _bulletCount;
    public float _distanceFromCaster;
    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(
        int maxUpgradePoint,
        float damage,
        float adRatio,
        float groggyDuration,

        float delay,
        float force,
        float bulletCount,
        float distanceFromCaster,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;

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
            _groggyDuration,
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
