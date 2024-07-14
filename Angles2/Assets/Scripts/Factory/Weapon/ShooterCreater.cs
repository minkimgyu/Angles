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
    public ShooterData(float damage, float moveSpeed, float shootForce, float fireDelay, float followOffset) : base(damage)
    {
        _moveSpeed = moveSpeed;
        _shootForce = shootForce;
        _fireDelay = fireDelay;
        _followOffset = followOffset;
    }
}

public class ShooterCreater : WeaponCreater<ShooterData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);

        return weapon;
    }
}
