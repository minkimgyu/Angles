using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShooterData : WeaponData
{
    private float _shootForce;
    private float _fireDelay;
    private BaseWeapon.Name _fireWeaponName; // --> 생성자가 아닌 스킬에서 설정해준다.
    private float _moveSpeed;
    private float _followOffset;
    private SerializableVector2 _followOffsetDirection;
    private float _maxDistanceFromPlayer;

    public float ShootForce { get => _shootForce; set => _shootForce = value; }
    [JsonIgnore] public float FireDelay { get => _fireDelay; set => _fireDelay = value; }
    public BaseWeapon.Name FireWeaponName { get => _fireWeaponName; set => _fireWeaponName = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public float FollowOffset { get => _followOffset; set => _followOffset = value; }
    public SerializableVector2 FollowOffsetDirection { get => _followOffsetDirection; set => _followOffsetDirection = value; }
    public float MaxDistanceFromPlayer { get => _maxDistanceFromPlayer; set => _maxDistanceFromPlayer = value; }

    public ShooterData(
        float shootForce,
        float fireDelay,
        float moveSpeed,
        float followOffset,
        SerializableVector2 followOffsetDirection,
        float maxDistanceFromPlayer
    )
    {
        _shootForce = shootForce;
        _fireDelay = fireDelay;

        _fireWeaponName = default;

        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        _followOffsetDirection = followOffsetDirection;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
    }

    public override WeaponData Copy()
    {
        return new ShooterData(
            _shootForce,
            _fireDelay,
            _moveSpeed,
            _followOffset,
            _followOffsetDirection,
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

        weapon.InjectData(CopyWeaponData as ShooterData);
        weapon.Initialize(_weaponFactory);

        return weapon;
    }
}
