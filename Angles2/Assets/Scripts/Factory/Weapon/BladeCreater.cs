using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BladeData : WeaponData, ILifetimeStat, ISizeModifyStat
{
    public float _attackDelay;
    public float _groggyDuration;

    public float Lifetime { get; set; }
    public float SizeMultiplier { get; set; }

    public BladeData(float attackDelay, float groggyDuration)
    {
        _attackDelay = attackDelay;
        _groggyDuration = groggyDuration;
    }

    public override void ChangeLifetime(float lifeTime) => Lifetime = lifeTime;
    public override void ChangeSizeMultiplier(float sizeMultiplier) => SizeMultiplier = sizeMultiplier;

    public override WeaponData Copy()
    {
        return new BladeData(
            _attackDelay,
            _groggyDuration
        );
    }
}

public class BladeCreater : WeaponCreater
{
    public BladeCreater(BaseWeapon weaponPrefab, WeaponData data) : base(weaponPrefab, data)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(CopyWeaponData as BladeData);
        weapon.Initialize();
        return weapon;
    }
}
