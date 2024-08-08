using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData : BaseWeaponData
{
    public float _lifeTime;
    public float _force;

    public ProjectileData(float damage, float lifeTime) : base(damage)
    {
        _lifeTime = lifeTime;
    }
}

[System.Serializable]
public class RocketData : ProjectileData
{
    public float _explosionDamage;
    public float _explosionRange;

    public RocketData(float damage, float lifeTime, float explosionDamage, float explosionRange) : base(damage, lifeTime)
    {
        _explosionDamage = explosionDamage;
        _explosionRange = explosionRange;
    }
}

public class RocketCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public RocketCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab, weaponData)
    {
        this.SpawnEffect = SpawnEffect;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        RocketData data = _weaponData as RocketData;
        weapon.Initialize(data, SpawnEffect);

        return weapon;
    }
}
