using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageModifier : WeaponDataModifier
{
    float _damageModifier;

    public WeaponDamageModifier(float damageModifier)
    {
        _damageModifier = damageModifier;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeDamage(_damageModifier);
        return weaponData;
    }
}

public class WeaponTotalDamageRatioModifier : WeaponDataModifier
{
    float _totalDamageRatio;

    public WeaponTotalDamageRatioModifier(float totalDamageRatio)
    {
        _totalDamageRatio = totalDamageRatio;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeTotalDamageRatio(_totalDamageRatio);
        return weaponData;
    }
}

public class WeaponTargetModifier : WeaponDataModifier
{
    List<ITarget.Type> _targetType;

    public WeaponTargetModifier(List<ITarget.Type> targetType)
    {
        _targetType = targetType;
    }

    public override T Visit<T>(T weaponData)
    {
        weaponData.ChangeTarget(_targetType);
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
