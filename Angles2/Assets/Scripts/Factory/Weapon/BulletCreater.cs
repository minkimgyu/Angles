using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletData : ProjectileData
{
    public BulletData(float damage, float lifeTime, float force) : base(damage, lifeTime, force)
    {
    }
}

public class BulletCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        BulletData data = Database.Instance.WeaponData[BaseWeapon.Name.Bullet] as BulletData;
        weapon.Initialize(data);
        return weapon;
    }
}
