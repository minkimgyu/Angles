using DamageUtility;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletData : WeaponData
{
    [JsonProperty] float _lifetime;
    public float Lifetime { get => _lifetime; set => _lifetime = value; }

    public BulletData(float lifeTime)
    {
        Lifetime = lifeTime;
    }

    public override WeaponData Copy()
    {
        return new BulletData(
            Lifetime
        );
    }
}

public class BulletCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public BulletCreater(BaseWeapon weaponPrefab, WeaponData weaponData, BaseFactory effectFactory) : base(weaponPrefab, weaponData)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.InjectData(CopyWeaponData as BulletData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
