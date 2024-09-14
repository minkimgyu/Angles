using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShooterUpgradableData
{
    public ShooterUpgradableData(float shootForce, float fireDelay)
    {
        _shootForce = shootForce; // 날리는 속도
        _fireDelay = fireDelay; // 연사 속도
    }

    private float _shootForce;
    private float _fireDelay;

    public float ShootForce { get => _shootForce; }
    public float FireDelay { get => _fireDelay; }
}

[System.Serializable]
public class ShooterData : WeaponData
{
    public float _damageRatio; // --> 이걸로 데미지 변경
    public float _shootForce;
    public float _fireDelay;
    public WeaponData _fireWeaponData;
    public BaseWeapon.Name _fireWeaponName;

    public float _moveSpeed;
    public float _followOffset;
    public float _maxDistanceFromPlayer;

    public static ShooterData operator +(ShooterData a, SpawnShooterUpgrader.UpgradableData b)
    {
        return new ShooterData(
            a.fireWeaponData, // 수정될 없음
            a._coolTime,
            a._maxStackCount,
            a._damage + b.Damage,
            a._range + b.Range,
            a._maxTargetCount + b.MaxTargetCount,
            a._targetTypes
        );
    }

    public ShooterData(WeaponData fireWeaponData, float moveSpeed, float followOffset, float maxDistanceFromPlayer, BaseWeapon.Name fireWeaponName)
    {
        _fireWeaponData = fireWeaponData;

        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
        _fireWeaponName = fireWeaponName;
    }
}

public class ShooterCreater : WeaponCreater
{
    BaseFactory _weaponFactory;

    public ShooterCreater(BaseWeapon weaponPrefab, BaseFactory weaponFactory) : base(weaponPrefab)
    {
        _weaponFactory = weaponFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize(_weaponFactory);
        return weapon;
    }
}
