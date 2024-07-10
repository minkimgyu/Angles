using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : BaseWeaponData
{
    public float _lifeTime;

    public float _absorbForce;
    public float _forceDelay;
    public int _maxTargetCount;

    public BlackholeData(float damage, float lifeTime, float absorbForce, float forceDelay, int maxTargetCount) : base(damage)
    {
        _lifeTime = lifeTime;
        _absorbForce = absorbForce;
        _forceDelay = forceDelay;
        _maxTargetCount = maxTargetCount;
    }
}

public class BlackholeCreater : WeaponCreater<BlackholeData>
{
    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_prefab);
        weapon.Initialize(_data);

        return weapon;
    }
}
