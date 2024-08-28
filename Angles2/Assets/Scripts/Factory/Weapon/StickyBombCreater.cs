using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StickyBombUpgradableData
{
    public StickyBombUpgradableData(float damage, float range, float explosionDelay)
    {
        _damage = damage;
        _range = range;
        _explosionDelay = explosionDelay;
    }

    private float _damage;
    private float _range;
    private float _explosionDelay;

    public float Damage { get => _damage; }
    public float Range { get => _range; }
    public float ExplosionDelay { get => _explosionDelay; }
}

[System.Serializable]
public class StickyBombData : BaseWeaponData
{
    public List<StickyBombUpgradableData> _upgradableDatas;

    public StickyBombData(List<StickyBombUpgradableData> upgradableDatas)
    {
        _upgradableDatas = upgradableDatas;
    }
}

public class StickyBombCreater : WeaponCreater
{
    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public StickyBombCreater(BaseWeapon weaponPrefab, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect) : base(weaponPrefab)
    {
        this.SpawnEffect = SpawnEffect;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize(SpawnEffect);
        return weapon;
    }
}
