using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BladeData : WeaponData
{
    public float _damage;
    public float _attackDelay;
    public float _range;
    public float _lifeTime;

    public static BladeData operator +(BladeData a, SpawnBladeUpgrader.UpgradableData b)
    {
        return new BladeData(
            a._damage + b._damage,
            a._attackDelay + b._attackDelay,
            a._range + b._range,
            a._lifeTime
        );
    }

    public BladeData(float damage, float attackDelay, float range, float lifeTime)
    {
        _damage = damage;
        _attackDelay = attackDelay;
        _range = range;
        _lifeTime = lifeTime;
    }
}

public class BladeCreater : WeaponCreater
{
    public BladeCreater(BaseWeapon weaponPrefab) : base(weaponPrefab)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize();
        return weapon;
    }
}
