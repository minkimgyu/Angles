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

public class RocketCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        RocketData data = Database.Instance.WeaponData[BaseWeapon.Name.Rocket] as RocketData;
        weapon.Initialize(data);
        return weapon;
    }
}
