using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : WeaponData, ILifetimeStat
{
    /// <summary>
    /// 적용되는 데미지
    /// </summary>
    public float _groggyDuration;

    /// <summary>
    /// 시전 범위
    /// </summary>
    public float _range;
    public float Lifetime { get; set; }

    public StickyBombData(float range, float lifetime)
    {
        _range = range;
        Lifetime = lifetime;
    }

    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }

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

        weapon.ResetData(CopyWeaponData as StickyBombData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
