using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : WeaponData, ILifetimeStat, ISizeModifyStat
{
    public float _absorbForce;
    public int _maxTargetCount;
    public float _forceDelay;

    public float Lifetime { get; set; }
    public float SizeMultiplier { get; set; }

    public BlackholeData(float lifeTime, float absorbForce, int maxTargetCount, float forceDelay)
    {
        Lifetime = lifeTime;
        _absorbForce = absorbForce;
        _maxTargetCount = maxTargetCount;
        _forceDelay = forceDelay;
    }

    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }
    public override void ChangeTargetCount(int targetCount) { _maxTargetCount = targetCount; }
    public override void ChangeForce(float force) { _absorbForce = force; }
    public override void ChangeSizeMultiplier(float sizeMultiplier) { sizeMultiplier = sizeMultiplier; }

    public override WeaponData Copy()
    {
        return new BlackholeData(
            Lifetime,
            _absorbForce,
            _maxTargetCount,
            _forceDelay
        );
    }
}

public class BlackholeCreater : WeaponCreater
{
    public BlackholeCreater(BaseWeapon weaponPrefab, WeaponData data) : base(weaponPrefab, data)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(_weaponData as BlackholeData);
        weapon.Initialize();

        return weapon;
    }
}
