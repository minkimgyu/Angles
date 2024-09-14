using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShooterUpgradableData
{
    public ShooterUpgradableData(float shootForce, float fireDelay)
    {
        _shootForce = shootForce; // ������ �ӵ�
        _fireDelay = fireDelay; // ���� �ӵ�
    }

    private float _shootForce;
    private float _fireDelay;

    public float ShootForce { get => _shootForce; }
    public float FireDelay { get => _fireDelay; }
}

[System.Serializable]
public class ShooterData : WeaponData
{
    public float _damageRatio; // --> �̰ɷ� ������ ����
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
            a.fireWeaponData, // ������ ����
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
