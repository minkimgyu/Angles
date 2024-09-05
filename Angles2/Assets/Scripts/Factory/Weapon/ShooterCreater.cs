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
public class ShooterData : BaseWeaponData
{
    public List<ShooterUpgradableData> _upgradableDatas;
    public BaseWeaponData _fireWeaponData;
    public BaseWeapon.Name _fireWeaponName;

    public float _moveSpeed;
    public float _followOffset;
    public float _maxDistanceFromPlayer;

    public ShooterData(List<ShooterUpgradableData> upgradableDatas, BaseWeaponData weaponData, float moveSpeed, float followOffset, float maxDistanceFromPlayer, BaseWeapon.Name fireWeaponName)
    {
        _upgradableDatas = upgradableDatas;
        _fireWeaponData = weaponData;

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
