using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletData : WeaponData, ILifetimeStat
{
    public float Lifetime { get; set; }
    public BulletData(float lifeTime)
    {
        Lifetime = lifeTime;
    }

    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }

    public override WeaponData Copy()
    {
        return new BulletData(
            Lifetime
        );
    }
}

public class BulletCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public BulletCreater(BaseWeapon weaponPrefab, WeaponData weaponData, BaseFactory effectFactory) : base(weaponPrefab, weaponData)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(CopyWeaponData as BulletData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}