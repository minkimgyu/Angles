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

public class BladeCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        BladeData data = Database.Instance.WeaponData[BaseWeapon.Name.Blade] as BladeData;
        weapon.Initialize(data);
        return weapon;
    }
}
