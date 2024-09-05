using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpreadBulletsData : BaseSkillData
{
    public BulletData _bulletData;
    public float _force;
    public float _delay;

    public float _distanceFromCaster;
    public int _bulletCount;

    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(int maxUpgradePoint, float force, float delay, float distanceFromCaster, int bulletCount, BulletData bulletData, List<ITarget.Type> targetTypes) : base(maxUpgradePoint) 
    {
        _bulletData = bulletData;
        _force = force;
        _delay = delay;
        _distanceFromCaster = distanceFromCaster;

        _bulletCount = bulletCount;

        _targetTypes = targetTypes;
    }
}

public class SpreadBulletsCreater : SkillCreater
{
    BaseFactory _weaponFactory;

    public SpreadBulletsCreater(BaseSkillData data, BaseFactory _weaponFactory) : base(data)
    {
        this._weaponFactory = _weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpreadBulletsData data = _buffData as SpreadBulletsData;
        return new SpreadBullets(data, _weaponFactory);
    }
}
