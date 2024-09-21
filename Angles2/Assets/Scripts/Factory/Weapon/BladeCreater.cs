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

    public BladeData(float attackDelay)
    {
        _damage = 0;
        _attackDelay = attackDelay;
        _range = 1;
        _lifeTime = 3;
    }

    public override void ChangeDamage(float damage) => _damage = damage;
    public override void ChangeRange(float range) => _range = range;
    public override void ChangeLifetime(float lifeTime) => _lifeTime = lifeTime;

    public override WeaponData Copy()
    {
        return new BladeData(
            _attackDelay
        );
    }
}

public class BladeCreater : WeaponCreater
{
    public BladeCreater(BaseWeapon weaponPrefab, WeaponData data) : base(weaponPrefab, data)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize();
        weapon.ResetData(_weaponData as BladeData);
        return weapon;
    }
}
