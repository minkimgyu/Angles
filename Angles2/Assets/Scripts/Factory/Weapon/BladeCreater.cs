using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BladeData : WeaponData, ILifetimeStat, ISizeModifyStat
{
    public float _damage;
    public float _attackDelay;
    public float Lifetime { get; set; }
    public float SizeMultiplier { get; set; }

    public BladeData(float attackDelay)
    {
        _damage = 0;
        _attackDelay = attackDelay;
        Lifetime = 3;
        SizeMultiplier = 1f;
    }

    public override void ChangeDamage(float damage) => _damage = damage;
    public override void ChangeLifetime(float lifeTime) => Lifetime = lifeTime;
    public override void ChangeSizeMultiplier(float sizeMultiplier) => SizeMultiplier = sizeMultiplier;

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

        weapon.ResetData(_weaponData as BladeData);
        weapon.Initialize();
        return weapon;
    }
}
