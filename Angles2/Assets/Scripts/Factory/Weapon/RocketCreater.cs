using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketData : WeaponData
{
    public float _explosionDamage;
    public int _explosionRange;
    public float _lifeTime;

    public RocketData(float explosionDamage, int explosionRange, float lifeTime)
    {
        _explosionDamage = explosionDamage;
        _explosionRange = explosionRange;
        _lifeTime = lifeTime;
    }

    public override void ChangeDamage(float damage) => _explosionDamage = damage;
    public override void ChangeRange(float range) => _explosionDamage = range;

    public override WeaponData Copy()
    {
        return new RocketData(
            _explosionDamage,
            _explosionRange,
            _lifeTime
        );
    }
}

public class RocketCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public RocketCreater(BaseWeapon weaponPrefab, WeaponData data, BaseFactory effectFactory) : base(weaponPrefab, data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize(_effectFactory);
        weapon.ResetData(_weaponData as RocketData);
        return weapon;
    }
}
