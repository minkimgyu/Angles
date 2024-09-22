using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : WeaponData, ILifetimeStat
{
    public float _damage;
    public float _range;
    public float Lifetime { get; set; }

    public StickyBombData(float range, float lifetime)
    {
        _damage = 0;
        _range = range;
        Lifetime = lifetime;
    }

    public override void ChangeDamage(float damage) { _damage = damage; }
    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }

    public override WeaponData Copy()
    {
        return new StickyBombData(
            _range,
            Lifetime
        );
    }
}

public class StickyBombCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public StickyBombCreater(BaseWeapon weaponPrefab, WeaponData data, BaseFactory effectFactory) : base(weaponPrefab, data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(_weaponData as StickyBombData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
