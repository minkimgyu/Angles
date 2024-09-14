using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : WeaponData
{
    public float _lifeTime;
    public float _absorbForce;
    public int _maxTargetCount;
    public float _range;
    public float _forceDelay;

    // + 연산자 오버로딩
    public static BlackholeData operator +(BlackholeData a, SpawnBlackholeUpgrader.UpgradableData b)
    {
        return new BlackholeData(
            a._lifeTime + b.LifeTime, // 수정될 없음
            a._absorbForce + b.AbsorbForce, // 수정될 없음
            a._maxTargetCount + b.MaxTargetCount,
            a._range + b.Range,
            a._forceDelay
        );
    }

    public BlackholeData(float lifeTime, float absorbForce, int maxTargetCount, float range, float forceDelay)
    {
        _lifeTime = lifeTime;
        _absorbForce = absorbForce;
        _maxTargetCount = maxTargetCount;
        _range = range;
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
