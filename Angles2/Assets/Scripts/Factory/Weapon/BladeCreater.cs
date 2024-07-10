using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BladeData : ProjectileData
{
    public float _attackDelay;

    public BladeData(float damage, float lifeTime, float force, float attackDelay) : base(damage, lifeTime, force)
    {
        _attackDelay = attackDelay;
    }
}

public class BladeCreater : WeaponCreater<BladeData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);

        return weapon;
    }
}
