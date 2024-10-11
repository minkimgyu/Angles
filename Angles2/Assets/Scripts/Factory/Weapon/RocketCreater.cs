using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketData : WeaponData, ILifetimeStat
{
    public float _damage;
    public float _range;
    public float Lifetime { get; set; }

    public RocketData(float range, float lifeTime)
    {
        _damage = 0;
        _range = range;
        Lifetime = lifeTime;
    }

    public override void ChangeDamage(float damage) => _damage = damage;
    public override void ChangeRange(float range) => _range = range;
    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }


    public override WeaponData Copy()
    {
        return new RocketData(
            _range,
            Lifetime
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

        weapon.ResetData(CopyWeaponData as RocketData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
