using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RocketUpgradableData
{
    public RocketUpgradableData(float damage, float explosionDamage, int explosionRange)
    {
        _damage = damage;
        _explosionDamage = explosionDamage;
        _explosionRange = explosionRange;
    }

    private float _damage;
    private float _explosionDamage;
    private int _explosionRange;

    public float Damage { get => _damage; }
    public float ExplosionDamage { get => _explosionDamage; }
    public int ExplosionRange { get => _explosionRange; }
}

[System.Serializable]
public class RocketData : BaseWeaponData
{
    public float _lifeTime;
    public List<RocketUpgradableData> _upgradableDatas;

    public RocketData(List<RocketUpgradableData> upgradableDatas, float lifeTime)
    {
        _upgradableDatas = upgradableDatas;
        _lifeTime = lifeTime;
    }
}

public class RocketCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public RocketCreater(BaseWeapon weaponPrefab, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab)
    {
        this.SpawnEffect = SpawnEffect;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize(SpawnEffect);
        return weapon;
    }
}
