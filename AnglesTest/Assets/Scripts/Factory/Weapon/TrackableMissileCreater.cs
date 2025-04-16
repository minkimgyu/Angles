using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrackableMissileData : WeaponData
{
    [JsonProperty] float _lifetime;
    public float Lifetime { get => _lifetime; set => _lifetime = value; }

    float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }

    public TrackableMissileData(float lifeTime, float moveSpeed)
    {
        Lifetime = lifeTime;
        _moveSpeed = moveSpeed;
    }

    public override WeaponData Copy()
    {
        return new TrackableMissileData(
            Lifetime,
            _moveSpeed
        );
    }
}

public class TrackableMissileCreater : WeaponCreater
{
    BaseFactory _effectFactory;

    public TrackableMissileCreater(BaseWeapon weaponPrefab, WeaponData data, BaseFactory effectFactory) : base(weaponPrefab, data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.InjectData(CopyWeaponData as TrackableMissileData);
        weapon.Initialize(_effectFactory);
        return weapon;
    }
}
