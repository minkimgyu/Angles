using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct SpreadBulletUpgradableData
{
    public SpreadBulletUpgradableData(float delay, float bulletCount)
    {
        _delay = delay;
        _bulletCount = bulletCount;
    }

    private float _delay;
    private float _bulletCount;

    public float Delay { get => _delay; }
    public float BulletCount { get => _bulletCount; }
}


[Serializable]
public class SpreadBulletsData : SkillData
{
    public BulletData _bulletData;
    public List<SpreadBulletUpgradableData> _upgradableDatas;

    public float _force;
    public float _distanceFromCaster;
    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(
        int maxUpgradePoint,
        float force,
        float distanceFromCaster,
        BulletData bulletData,
        List<SpreadBulletUpgradableData> upgradableDatas,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint) 
    {
        _force = force;
        _distanceFromCaster = distanceFromCaster;
        _bulletData = bulletData;

        _upgradableDatas = upgradableDatas;
        _targetTypes = targetTypes;
    }
}

public class SpreadBulletsCreater : SkillCreater
{
    BaseFactory _weaponFactory;

    public SpreadBulletsCreater(SkillData data, BaseFactory _weaponFactory) : base(data)
    {
        this._weaponFactory = _weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpreadBulletsData data = _skillData as SpreadBulletsData;
        return new SpreadBullets(data, _weaponFactory);
    }
}
