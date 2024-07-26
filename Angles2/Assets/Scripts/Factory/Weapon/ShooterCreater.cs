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

public class ShooterCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        ShooterData data = Database.Instance.WeaponData[BaseWeapon.Name.Shooter] as ShooterData;
        weapon.Initialize(data);
        return weapon;
    }
}
