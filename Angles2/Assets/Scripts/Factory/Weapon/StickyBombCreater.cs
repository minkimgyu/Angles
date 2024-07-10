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

public class StickyBombCreater : WeaponCreater<StickyBombData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);
        return weapon;
    }
}
