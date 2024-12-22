using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : WeaponData, ILifetimeStat, ISizeModifyStat
{
    [JsonProperty] private float _absorbForce;
    [JsonProperty] private float _forceDelay;

    [JsonIgnore] public float AbsorbForce { get => _absorbForce; set => _absorbForce = value; }
    [JsonIgnore] public float ForceDelay { get => _forceDelay; set => _forceDelay = value; }

    public float Lifetime { get; set; }
    public float SizeMultiplier { get; set; }

    public BlackholeData(float force, float forceDelay)
    {
        _absorbForce = force;
        _forceDelay = forceDelay;
    }

    public override void ChangeLifetime(float lifetime) { Lifetime = lifetime; }
    public override void ChangeSizeMultiplier(float sizeMultiplier) { SizeMultiplier = sizeMultiplier; }

    public override WeaponData Copy()
    {
        return new BlackholeData(
            _absorbForce,
            _forceDelay
        );
    }
}

public class BlackholeCreater : WeaponCreater
{
    public BlackholeCreater(BaseWeapon weaponPrefab, WeaponData data) : base(weaponPrefab, data)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.ResetData(CopyWeaponData as BlackholeData);
        weapon.Initialize();

        return weapon;
    }
}
