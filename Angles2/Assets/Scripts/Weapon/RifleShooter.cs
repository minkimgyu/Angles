using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : Shooter
{
    protected override BaseWeapon ReturnProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(_shooterData._fireWeaponName);
        weapon.ResetData(_shooterData._fireWeaponData as BulletData);
        weapon.ResetPosition(transform.position);
        weapon.ResetTargetTypes(_targetTypes);

        return weapon;
    }
}
