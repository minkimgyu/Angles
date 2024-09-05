using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletUpgradableData
{
    public BulletUpgradableData(float damage)
    {
        _damage = damage;
    }

    private float _damage;

    public float Damage { get => _damage; }
}

[System.Serializable]
public class BulletData : BaseWeaponData
{
    public float _lifeTime;
    public List<BulletUpgradableData> _upgradableDatas;

    public BulletData(List<BulletUpgradableData> upgradableDatas, float lifeTime)
    {
        _upgradableDatas = upgradableDatas;
        _lifeTime = lifeTime;
    }
}

public class BulletCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public BulletCreater(BaseWeapon weaponPrefab, BaseFactory effectFactory) : base(weaponPrefab)
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
