using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooterAttackStrategy : ShooterAttackStrategy
{
    BaseFactory _weaponFactory;

    public RifleShooterAttackStrategy(
        ShooterData shooterData,
        Transform myTransform,
        Func<List<ITarget>> GetTargets,
        BaseFactory weaponFactory) : base(shooterData, myTransform, GetTargets)
    {
        _weaponFactory = weaponFactory;
    }

    protected override BaseWeapon CreateProjectileWeapon()
    {
        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.ShooterBullet);
        BulletDataModifier bulletDataModifier = new BulletDataModifier(_shooterData.DamageableStat, _shooterData.TargetTypes);

        weapon.ModifyData(bulletDataModifier);
        return weapon;
    }
}
