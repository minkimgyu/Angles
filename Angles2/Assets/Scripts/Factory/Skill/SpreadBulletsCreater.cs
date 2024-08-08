using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpreadBulletsData : BaseSkillData
{
    public float _damage;
    public float _force;
    public float _delay;

    public float _distanceFromCaster;
    public int _bulletCount;

    public List<ITarget.Type> _targetTypes;

    public SpreadBulletsData(int maxUpgradePoint, float damage, float force, float delay, float distanceFromCaster, int bulletCount, List<ITarget.Type> targetTypes) : base(maxUpgradePoint) 
    {
        _damage = damage;
        _force = force;
        _delay = delay;
        _distanceFromCaster = distanceFromCaster;

        _bulletCount = bulletCount;

        _targetTypes = targetTypes;
    }
}

public class SpreadBulletsCreater : SkillCreater
{
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpreadBulletsCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpreadBulletsData data = _skillData as SpreadBulletsData;
        return new SpreadBullets(data, CreateWeapon);
    }
}
