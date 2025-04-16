using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BlackholeDataModifier : WeaponDataModifier<BlackholeData>
{
    DamageableData _damageableDataModifier;
    float _sizeModifier;
    float _lifeTime;
    List<ITarget.Type> _targetTypes;

    public BlackholeDataModifier(
        DamageableData damageableDataModifier,
        float sizeModifier,
        float lifeTime,
        List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _sizeModifier = sizeModifier;
        _lifeTime = lifeTime;
        _targetTypes = targetTypes;
    }

    public override void Visit(BlackholeData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.SizeMultiplier = _sizeModifier;
        weaponData.Lifetime = _lifeTime;
        weaponData.TargetTypes = _targetTypes;
    }
}





public class StickyBombDataModifier : WeaponDataModifier<StickyBombData>
{
    DamageableData _damageableDataModifier;
    List<ITarget.Type> _targetTypes;

    public StickyBombDataModifier(DamageableData damageableDataModifier, List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _targetTypes = targetTypes;
    }

    public override void Visit(StickyBombData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.TargetTypes = _targetTypes;
    }
}





public class ShooterDataModifier : WeaponDataModifier<ShooterData>
{
    DamageableData _damageableDataModifier;
    float _fireDelay;
    List<ITarget.Type> _targetTypes;

    public ShooterDataModifier(DamageableData damageableDataModifier, float fireDelay, List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _fireDelay = fireDelay;
        _targetTypes = targetTypes;
    }

    public override void Visit(ShooterData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.FireDelay = _fireDelay;
        weaponData.TargetTypes = _targetTypes;
    }
}




public class RocketDataModifier : WeaponDataModifier<RocketData>
{
    DamageableData _damageableDataModifier;
    List<ITarget.Type> _targetTypes;

    public RocketDataModifier(DamageableData damageableDataModifier, List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _targetTypes = targetTypes;
    }

    public override void Visit(RocketData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.TargetTypes = _targetTypes;
    }
}




public class BulletDataModifier : WeaponDataModifier<BulletData>
{
    DamageableData _damageableDataModifier;
    List<ITarget.Type> _targetTypes;

    public BulletDataModifier(DamageableData damageableDataModifier, List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _targetTypes = targetTypes;
    }

    public override void Visit(BulletData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.TargetTypes = _targetTypes;
    }
}

public class TrackableMissileDataModifier : WeaponDataModifier<TrackableMissileData>
{
    DamageableData _damageableDataModifier;
    List<ITarget.Type> _targetTypes;

    public TrackableMissileDataModifier(DamageableData damageableDataModifier, List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _targetTypes = targetTypes;
    }

    public override void Visit(TrackableMissileData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.TargetTypes = _targetTypes;
    }
}

public class BladeDataModifier : WeaponDataModifier<BladeData>
{
    DamageableData _damageableDataModifier;
    float _sizeMultiplier;
    float _lifeTime;
    List<ITarget.Type> _targetTypes;

    public BladeDataModifier(
        DamageableData damageableDataModifier,
        float sizeMultiplier,
        float lifeTime,
        List<ITarget.Type> targetTypes)
    {
        _damageableDataModifier = damageableDataModifier;
        _sizeMultiplier = sizeMultiplier;
        _lifeTime = lifeTime;
        _targetTypes = targetTypes;
    }

    public override void Visit(BladeData weaponData)
    {
        weaponData.DamageableStat = _damageableDataModifier;
        weaponData.SizeMultiplier = _sizeMultiplier;
        weaponData.Lifetime = _lifeTime;
        weaponData.TargetTypes = _targetTypes;
    }
}

abstract public class WeaponDataModifier<T> where T : WeaponData
{
    public abstract void Visit(T weaponData);
}
