using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShooterData : BaseWeaponData
{
    public float _moveSpeed;
    public float _shootForce;
    public float _fireDelay;
    public float _followOffset;
    public float _maxDistanceFromPlayer;
    public BaseWeapon.Name _fireWeaponName;

    public ShooterData(float damage, float moveSpeed, float shootForce, float fireDelay, float followOffset, float maxDistanceFromPlayer, BaseWeapon.Name fireWeaponName) : base(damage)
    {
        _moveSpeed = moveSpeed;
        _shootForce = shootForce;
        _fireDelay = fireDelay;
        _followOffset = followOffset;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
        _fireWeaponName = fireWeaponName;
    }
}

public class ShooterCreater : WeaponCreater
{
    System.Func<BaseWeapon.Name, BaseWeapon> SpawnWeapon;

    public ShooterCreater(BaseWeapon weaponPrefab, BaseWeaponData weaponData, System.Func<BaseWeapon.Name, BaseWeapon> SpawnWeapon) : base(weaponPrefab, weaponData)
    {
        this.SpawnWeapon = SpawnWeapon;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        ShooterData data = _weaponData as ShooterData;
        weapon.Initialize(data, SpawnWeapon);
        return weapon;
    }
}
