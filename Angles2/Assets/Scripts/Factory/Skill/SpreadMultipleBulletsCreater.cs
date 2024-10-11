using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpreadMultipleBulletsData : SkillData
{
    public float _waveDelay;
    public int _maxWaveCount;

    public float _damage;

    public float _delay;
    public float _force;
    public float _bulletCount;
    public float _distanceFromCaster;
    public List<ITarget.Type> _targetTypes;

    public SpreadMultipleBulletsData(
        int maxUpgradePoint, 
        float waveDelay, 
        int maxWaveCount,

        float damage,
        float delay,
        float force,
        float bulletCount,
        float distanceFromCaster,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _waveDelay = waveDelay;
        _maxWaveCount = maxWaveCount;

        _damage = damage;

        _delay = delay;
        _force = force;
        _bulletCount = bulletCount;
        _distanceFromCaster = distanceFromCaster;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpreadMultipleBulletsData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _waveDelay,
            _maxWaveCount,
            _damage,
            _delay,
            _force,
            _bulletCount,
            _distanceFromCaster,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
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
