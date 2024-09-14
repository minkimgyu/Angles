using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : WeaponData
{
    public float _damage;
    public float _range;
    public float _explosionDelay;

    // + 연산자 오버로딩
    public static StickyBombData operator +(StickyBombData a, SpawnStickyBombUpgrader.UpgradableData b)
    {
        return new StickyBombData(
            a._damage + b._damage,
            a._range + b._range,
            a._explosionDelay + b._explosionDelay
        );
    }

    public StickyBombData(float damage, float range, float explosionDelay)
    {
        _damage = damage;
        _range = range;
        _explosionDelay = explosionDelay;
    }
}

public class StickyBombCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public StickyBombCreater(BaseWeapon weaponPrefab, BaseFactory effectFactory) : base(weaponPrefab)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
