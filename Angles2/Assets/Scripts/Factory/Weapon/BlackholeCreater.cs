using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : WeaponData
{
    public float _lifeTime;
    public float _absorbForce;
    public int _maxTargetCount;
    public float _range;
    public float _forceDelay;

    public BlackholeData(float lifeTime, float absorbForce, int maxTargetCount, float range, float forceDelay)
    {
        _lifeTime = lifeTime;
        _absorbForce = absorbForce;
        _maxTargetCount = maxTargetCount;
        _range = range;
        _forceDelay = forceDelay;
    }

    public override void ChangeRange(float range) { _range = range; }
    public override void ChangeLifetime(float lifetime) { _lifeTime = lifetime; }
    public override void ChangeTargetCount(int targetCount) { _maxTargetCount = targetCount; }
    public override void ChangeForce(float force) { _absorbForce = force; }

    public override WeaponData Copy()
    {
        return new BlackholeData(
            _lifeTime,
            _absorbForce,
            _maxTargetCount,
            _range,
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

        weapon.Initialize();
        weapon.ResetData(_weaponData as BlackholeData);

        return weapon;
    }
}
