using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletData : WeaponData
{
    public float _damage;
    public float _lifeTime;

    public BulletData(float damage, float lifeTime)
    {
        _damage = damage;
        _lifeTime = lifeTime;
    }

    public override void ChangeDamage(float damage) => _damage = damage;

    public override WeaponData Copy()
    {
        return new BulletData(
            _damage,
            _lifeTime
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

        weapon.Initialize(_effectFactory);
        weapon.ResetData(_weaponData as BulletData);
        return weapon;
    }
}
