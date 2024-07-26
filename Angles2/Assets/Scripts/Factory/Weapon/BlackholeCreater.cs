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

public class BlackholeCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        BlackholeData data = Database.Instance.WeaponData[BaseWeapon.Name.Blackhole] as BlackholeData;
        weapon.Initialize(data);
        return weapon;
    }
}
