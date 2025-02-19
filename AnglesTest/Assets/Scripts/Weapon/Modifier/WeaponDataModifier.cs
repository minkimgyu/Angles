using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class WeaponDamageModifier : WeaponDataModifier
//{
//    DamageableData _damageableDataModifier;

//    public WeaponDamageModifier(DamageableData damageableDataModifier)
//    {
//        _damageableDataModifier = damageableDataModifier;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeDamage(_damageableDataModifier);
//        return weaponData;
//    }
//}

//public class WeaponDelayModifier : WeaponDataModifier
//{
//    float _delayModifier;

//    public WeaponDelayModifier(float delayModifier)
//    {
//        _delayModifier = delayModifier;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeDelay(_delayModifier);
//        return weaponData;
//    }
//}

//public class WeaponSizeModifier : WeaponDataModifier
//{
//    float _rangeModifier;

//    public WeaponSizeModifier(float rangeModifier)
//    {
//        _rangeModifier = rangeModifier;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeSizeMultiplier(_rangeModifier);
//        return weaponData;
//    }
//}

//public class WeaponTargetCountModifier : WeaponDataModifier
//{
//    int _targetCountModifier;

//    public WeaponTargetCountModifier(int targetCountModifier)
//    {
//        _targetCountModifier = targetCountModifier;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeTargetCount(_targetCountModifier);
//        return weaponData;
//    }
//}

//public class WeaponForceModifier : WeaponDataModifier
//{
//    float _forceModifier;

//    public WeaponForceModifier(float forceModifier)
//    {
//        _forceModifier = forceModifier;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeForce(_forceModifier);
//        return weaponData;
//    }
//}

//public class WeaponLifetimeModifier : WeaponDataModifier
//{
//    float _lifetime;

//    public WeaponLifetimeModifier(float lifetime)
//    {
//        _lifetime = lifetime;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeLifetime(_lifetime);
//        return weaponData;
//    }
//}

//public class WeaponProjectileModifier : WeaponDataModifier
//{
//    BaseWeapon.Name name;

//    public WeaponProjectileModifier(BaseWeapon.Name name)
//    {
//        this.name = name;
//    }

//    public override T Visit<T>(T weaponData)
//    {
//        weaponData.ChangeProjectile(name);
//        return weaponData;
//    }
//}













public class BlackholeDataModifier : WeaponDataModifier<BlackholeData>
{
    DamageableData _damageableDataModifier;
    float _sizeModifier;
    float _lifeTime;

    public BlackholeDataModifier(DamageableData damageableDataModifier, float sizeModifier, float lifeTime)
    {
        _damageableDataModifier = damageableDataModifier;
        _sizeModifier = sizeModifier;
        _lifeTime = lifeTime;
    }

    public override void Visit(BlackholeData weaponData)
    {
        weaponData.DamageableData = _damageableDataModifier;
        weaponData.SizeMultiplier = _sizeModifier;
        weaponData.Lifetime = _lifeTime;
    }
}




public class StickyBombDataModifier : WeaponDataModifier<StickyBombData>
{
    DamageableData _damageableDataModifier;
    float _lifeTime;

    public StickyBombDataModifier(DamageableData damageableDataModifier, float lifeTime)
    {
        _damageableDataModifier = damageableDataModifier;
        _lifeTime = lifeTime;
    }

    public override void Visit(StickyBombData weaponData)
    {
        weaponData.DamageableData = _damageableDataModifier;
        weaponData.Lifetime = _lifeTime;
    }
}





public class ShooterDataModifier : WeaponDataModifier<ShooterData>
{
    DamageableData _damageableDataModifier;
    float _fireDelay;

    public ShooterDataModifier(DamageableData damageableDataModifier, float fireDelayModifier)
    {
        _damageableDataModifier = damageableDataModifier;
        _fireDelay = fireDelayModifier;
    }

    public override void Visit(ShooterData weaponData)
    {
        weaponData.DamageableData = _damageableDataModifier;
        weaponData.FireDelay = _fireDelay;
    }
}


public class RocketDataModifier : WeaponDataModifier<RocketData>
{
    DamageableData _damageableDataModifier;

    public RocketDataModifier(DamageableData damageableDataModifier)
    {
        _damageableDataModifier = damageableDataModifier;
    }

    public override void Visit(RocketData attackStat)
    {
        attackStat.DamageableData = _damageableDataModifier;
    }
}

public class BulletDataModifier : WeaponDataModifier<BulletData>
{
    DamageableData _damageableDataModifier;

    public BulletDataModifier(DamageableData damageableDataModifier)
    {
        _damageableDataModifier = damageableDataModifier;
    }

    public override void Visit(BulletData bulletData)
    {
        bulletData.DamageableData = _damageableDataModifier;
    }
}

public class TrackableMissileDataModifier : WeaponDataModifier<TrackableMissileData>
{
    DamageableData _damageableDataModifier;

    public TrackableMissileDataModifier(DamageableData damageableDataModifier)
    {
        _damageableDataModifier = damageableDataModifier;
    }

    public override void Visit(TrackableMissileData missileData)
    {
        missileData.DamageableData = _damageableDataModifier;
    }
}


public class BladeDataModifier : WeaponDataModifier<BladeData>
{
    DamageableData _damageableDataModifier;
    float _sizeMultiplier;
    float _lifeTime;

    public BladeDataModifier(DamageableData damageableDataModifier, float sizeMultiplier, float lifeTime)
    {
        _damageableDataModifier = damageableDataModifier;
        _sizeMultiplier = sizeMultiplier;
        _lifeTime = lifeTime;
    }

    public override void Visit(BladeData weaponData)
    {
        weaponData.DamageableData = _damageableDataModifier;
        weaponData.SizeMultiplier = _sizeMultiplier;
        weaponData.Lifetime = _lifeTime;
    }
}

abstract public class WeaponDataModifier<T> where T : WeaponData
{
    public abstract void Visit(T weaponData);
}
