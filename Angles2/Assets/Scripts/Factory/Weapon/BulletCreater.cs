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

public class BulletCreater : WeaponCreater<BulletData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);

        return weapon;
    }
}
