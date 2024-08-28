using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletUpgradableData
{
    public BulletUpgradableData(float damage)
    {
        _damage = damage;
    }

    private float _damage;

    public float Damage { get => _damage; }
}

[System.Serializable]
public class BulletData : BaseWeaponData
{
    public float _lifeTime;
    public List<BulletUpgradableData> _upgradableDatas;

    public BulletData(List<BulletUpgradableData> upgradableDatas, float lifeTime)
    {
        _upgradableDatas = upgradableDatas;
        _lifeTime = lifeTime;
    }
}

public class BulletCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public BulletCreater(BaseWeapon weaponPrefab, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab)
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
