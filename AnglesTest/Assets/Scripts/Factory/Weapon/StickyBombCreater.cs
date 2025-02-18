using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : WeaponData, ILifetimeStat
{
    /// <summary>
    /// 시전 범위
    /// </summary>
    [JsonProperty] private float _range;
    [JsonIgnore] public float Range { get => _range; set => _range = value; }

    public float Lifetime { get; set; }

    public StickyBombData(float range, float lifetime)
    {
        _range = range;
        Lifetime = lifetime;
    }

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

        weapon.InjectData(CopyWeaponData as StickyBombData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
