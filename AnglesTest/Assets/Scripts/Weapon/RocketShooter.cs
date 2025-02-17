using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooter : BaseShooter
{
    protected override BaseWeapon CreateProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.ShooterRocket);
        RocketDataModifier rocketDataModifier = new RocketDataModifier(_data.DamageableData);

        weapon.ModifyData(rocketDataModifier);
        return weapon;
    }
}
