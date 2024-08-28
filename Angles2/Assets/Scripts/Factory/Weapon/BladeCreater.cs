using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BladeUpgradableData
{
    public BladeUpgradableData(float damage, float attackDelay, float range)
    {
        _damage = damage;
        _attackDelay = attackDelay;
        _range = range;
    }

    private float _damage;
    private float _attackDelay;
    private float _range;

    public float Damage { get => _damage; }
    public float AttackDelay { get => _attackDelay; }
    public float Range { get => _range; }
}

[System.Serializable]
public class BladeData : BaseWeaponData
{
    public float _lifeTime;
    public List<BladeUpgradableData> _upgradableDatas;

    public BladeData(List<BladeUpgradableData> upgradableDatas, float lifeTime)
    {
        _upgradableDatas = upgradableDatas;
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
