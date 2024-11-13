using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageModifier : WeaponDataModifier
{
    DamageableData _damageableDataModifier;

    public WeaponDamageModifier(DamageableData damageableDataModifier)
    {
        _damageableDataModifier = damageableDataModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeDamage(_damageableDataModifier);
        return weaponData;
    }
}

public class WeaponDelayModifier : WeaponDataModifier
{
    float _delayModifier;

    public WeaponDelayModifier(float delayModifier)
    {
        _delayModifier = delayModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeDelay(_delayModifier);
        return weaponData;
    }
}

public class WeaponSizeModifier : WeaponDataModifier
{
    float _rangeModifier;

    public WeaponSizeModifier(float rangeModifier)
    {
        _rangeModifier = rangeModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeSizeMultiplier(_rangeModifier);
        return weaponData;
    }
}

public class WeaponTargetCountModifier : WeaponDataModifier
{
    int _targetCountModifier;

    public WeaponTargetCountModifier(int targetCountModifier)
    {
        _targetCountModifier = targetCountModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeTargetCount(_targetCountModifier);
        return weaponData;
    }
}

public class WeaponForceModifier : WeaponDataModifier
{
    float _forceModifier;

    public WeaponForceModifier(float forceModifier)
    {
        _forceModifier = forceModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeForce(_forceModifier);
        return weaponData;
    }
}

public class WeaponLifetimeModifier : WeaponDataModifier
{
    float _lifetime;

    public WeaponLifetimeModifier(float lifetime)
    {
        _lifetime = lifetime;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeLifetime(_lifetime);
        return weaponData;
    }
}

public class WeaponProjectileModifier : WeaponDataModifier
{
    BaseWeapon.Name name;

    public WeaponProjectileModifier(BaseWeapon.Name name)
    {
        this.name = name;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeProjectile(name);
        return weaponData;
    }
}

abstract public class WeaponDataModifier
{
    public abstract T Visit<T>(T weaponData) where T : WeaponData;
}
