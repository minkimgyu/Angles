using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : BaseShooter
{
    protected override BaseWeapon CreateProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.ShooterBullet);
        BulletDataModifier bulletDataModifier = new BulletDataModifier(_data.DamageableData);

        weapon.ModifyData(bulletDataModifier);
        return weapon;
    }
}
