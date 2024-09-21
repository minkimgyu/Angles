using DamageUtility;
using System;
using System.Collections.Generic;

[Serializable]
public class SpreadBulletsData : SkillData
{
    public float _damage;

    public float _delay;
    public float _force;
    public float _bulletCount;
    public float _distanceFromCaster;
    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(
        float damage,

        float delay,
        float force,
        float bulletCount,
        float distanceFromCaster,
        List<ITarget.Type> targetTypes)
    {
        _damage = damage;

        _delay = delay;
        _force = force;
        _bulletCount = bulletCount;
        _distanceFromCaster = distanceFromCaster;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpreadBulletsData(
            _damage,
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
        SpreadBulletsData data = _skillData as SpreadBulletsData;
        return new SpreadBullets(data, _weaponFactory);
    }
}
