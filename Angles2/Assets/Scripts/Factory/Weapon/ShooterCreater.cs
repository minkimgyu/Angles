using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShooterData : BaseWeaponData
{
    public float _moveSpeed;
    public float _fireMaxDelay;
    public float _offsetToFollower;

    public ShooterData(float damage, float moveSpeed, float fireMaxDelay, float offsetToFollower) : base(damage)
    {
        _moveSpeed = moveSpeed;
        _fireMaxDelay = fireMaxDelay;
        _offsetToFollower = offsetToFollower;
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
