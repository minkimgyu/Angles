using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileData : BaseWeaponData
{
    public float _lifeTime;
    public float _force;

    public ProjectileData(float damage, float lifeTime, float force) : base(damage)
    {
        _lifeTime = lifeTime;
        _force = force;
    }
}

[System.Serializable]
public class RocketData : ProjectileData
{
    public float _explosionDamage;
    public float _explosionRange;

    public RocketData(float damage, float lifeTime, float force, float explosionDamage, float explosionRange) : base(damage, lifeTime, force)
    {
        _explosionDamage = explosionDamage;
        _explosionRange = explosionRange;
    }
}

public class RocketCreater : WeaponCreater<RocketData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);

        return weapon;
    }
}
