using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrackableMissileData : WeaponData, ILifetimeStat
{
    public float Lifetime { get; set; }

    public TrackableMissileData(float lifeTime)
    {
        Lifetime = lifeTime;
    }

    public override WeaponData Copy()
    {
        return new TrackableMissileData(
            Lifetime
        );
    }
}

public class TrackableMissileCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public TrackableMissileCreater(BaseWeapon weaponPrefab, WeaponData data, BaseFactory effectFactory) : base(weaponPrefab, data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.InjectData(CopyWeaponData as TrackableMissileData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
