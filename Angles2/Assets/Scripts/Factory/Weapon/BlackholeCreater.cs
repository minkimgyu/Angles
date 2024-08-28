using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BlackholeUpgradableData
{
    public BlackholeUpgradableData(float lifeTime, float absorbForce, int maxTargetCount, float range)
    {
        _lifeTime = lifeTime;
        _absorbForce = absorbForce;
        _maxTargetCount = maxTargetCount;
        _range = range;
    }

    private float _lifeTime;
    private float _absorbForce;
    private int _maxTargetCount;
    private float _range;

    public float LifeTime { get => _lifeTime; }
    public float AbsorbForce { get => _absorbForce; }
    public int MaxTargetCount { get => _maxTargetCount; }
    public float Range { get => _range; }
}

[System.Serializable]
public class BlackholeData : BaseWeaponData
{
    public List<BlackholeUpgradableData> _upgradableDatas;
    public float _forceDelay;

    public BlackholeData(List<BlackholeUpgradableData> upgradableDatas, float forceDelay)
    {
        _upgradableDatas = upgradableDatas;
        _forceDelay = forceDelay;
    }
}

public class BlackholeCreater : WeaponCreater
{
    public BlackholeCreater(BaseWeapon weaponPrefab) : base(weaponPrefab)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.Initialize();
        return weapon;
    }
}
