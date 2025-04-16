using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooterAttackStrategy : ShooterAttackStrategy
{
    BaseFactory _weaponFactory;

    public RocketShooterAttackStrategy(
        ShooterData shooterData,
        Transform myTransform,
        Func<List<ITarget>> GetTargets,
        BaseFactory weaponFactory) : base(shooterData, myTransform, GetTargets)
    {
        _weaponFactory = weaponFactory;
    }

    protected override BaseWeapon CreateProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.ShooterRocket);
        RocketDataModifier rocketDataModifier = new RocketDataModifier(_shooterData.DamageableStat, _shooterData.TargetTypes);

        weapon.ModifyData(rocketDataModifier);
        return weapon;
    }
}
