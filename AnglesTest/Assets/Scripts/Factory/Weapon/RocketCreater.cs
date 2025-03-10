using DamageUtility;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketData : WeaponData, ILifetimeStat
{
    [JsonProperty] private float _range;
    [JsonIgnore] public float Range { get => _range; set => _range = value; }

    public float Lifetime { get; set; }

    public RocketData(float range, float lifeTime)
    {
        _range = range;
        Lifetime = lifeTime;
    }

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

        weapon.InjectData(CopyWeaponData as RocketData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
