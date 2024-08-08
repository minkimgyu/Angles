using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BladeData : ProjectileData
{
    public float _attackDelay;

    public BladeData(float damage, float lifeTime, float attackDelay) : base(damage, lifeTime)
    {
        _attackDelay = attackDelay;
    }
}

public class BladeCreater : WeaponCreater
{
    public BladeCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData) : base(weaponPrefab, weaponData)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        BladeData data = _weaponData as BladeData;
        weapon.Initialize(data);
        return weapon;
    }
}
