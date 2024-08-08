using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletData : ProjectileData
{
    public BulletData(float damage, float lifeTime) : base(damage, lifeTime)
    {
    }
}

public class BulletCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public BulletCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab, weaponData)
    {
        this.SpawnEffect = SpawnEffect;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        BulletData data = _weaponData as BulletData;
        weapon.Initialize(data, SpawnEffect);
        return weapon;
    }
}
