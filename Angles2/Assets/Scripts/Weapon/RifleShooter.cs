using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : Shooter
{
    BulletData ProjectileData { get { return _weaponData as BulletData; } }

    protected override BaseWeapon ReturnProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(_fireWeaponName);
        weapon.ResetData(ProjectileData);
        weapon.ResetPosition(transform.position);
        weapon.ResetTargetTypes(_targetTypes);

        return weapon;
    }
}
