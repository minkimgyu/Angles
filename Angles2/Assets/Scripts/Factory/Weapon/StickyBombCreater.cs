using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : WeaponData
{
    public float _damage;
    public float _range;
    public float _explosionDelay;

    public StickyBombData(float damage, float range, float explosionDelay)
    {
        _damage = damage;
        _range = range;
        _explosionDelay = explosionDelay;
    }

    public override void ChangeDamage(float damage) { _damage = damage; }
    public override void ChangeDelay(float delay) { _explosionDelay = delay; }

    public override WeaponData Copy()
    {
        return new StickyBombData(
            _damage,
            _range,
            _explosionDelay
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

        weapon.Initialize(_effectFactory);
        weapon.ResetData(_weaponData as StickyBombData);
        return weapon;
    }
}
