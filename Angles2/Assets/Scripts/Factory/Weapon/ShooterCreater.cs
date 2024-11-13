using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShooterData : WeaponData
{
    public float _shootForce;
    public float _fireDelay;
    public BaseWeapon.Name _fireWeaponName; // --> 생성자가 아닌 스킬에서 설정해준다.

    public float _moveSpeed;
    public float _followOffset;
    public float _maxDistanceFromPlayer;

    public ShooterData(float shootForce, float fireDelay, float moveSpeed, float followOffset, float maxDistanceFromPlayer)
    {
        _shootForce = shootForce;
        _fireDelay = fireDelay;

        _fireWeaponName = default;

        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
    }

    public override void ChangeDelay(float delay) { _fireDelay = delay; }
    public override void ChangeProjectile(BaseWeapon.Name name) { _fireWeaponName = name; }

    public override WeaponData Copy()
    {
        return new ShooterData(
            _shootForce,
            _fireDelay,
            _moveSpeed,
            _followOffset,
            _maxDistanceFromPlayer
        );
    }
}

public class ShooterCreater : WeaponCreater
{
    BaseFactory _weaponFactory;

    public ShooterCreater(BaseWeapon weaponPrefab, WeaponData data, BaseFactory weaponFactory) : base(weaponPrefab, data)
    {
        _weaponFactory = weaponFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(CopyWeaponData as ShooterData);
        weapon.Initialize(_weaponFactory);

        return weapon;
    }
}
