using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooterAttackStrategy : ShooterAttackStrategy
{
    BaseFactory _weaponFactory;

    public RocketShooterAttackStrategy(
        ShooterData shooterData,
        Transform myTransform,
        BaseFactory weaponFactory) : base(shooterData, myTransform)
    {
        _weaponFactory = weaponFactory;
    }

    protected override BaseWeapon CreateProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.ShooterRocket);
        RocketDataModifier rocketDataModifier = new RocketDataModifier(_shooterData.DamageableData);

        weapon.ModifyData(rocketDataModifier);
        return weapon;
    }
}
