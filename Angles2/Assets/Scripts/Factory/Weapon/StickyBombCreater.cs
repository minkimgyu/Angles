using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : BaseWeaponData
{
    public float _range;
    public float _explosionDelay;

    public StickyBombData(float damage, float range, float explosionDelay) : base(damage)
    {
        _range = range;
        _explosionDelay = explosionDelay;
    }
}

public class StickyBombCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public StickyBombCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab, weaponData)
    {
        this.SpawnEffect = SpawnEffect;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        StickyBombData data = _weaponData as StickyBombData;
        weapon.Initialize(data, SpawnEffect);
        return weapon;
    }
}
