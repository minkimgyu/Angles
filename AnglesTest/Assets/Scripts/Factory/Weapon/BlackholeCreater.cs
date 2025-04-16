using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlackholeData : WeaponData
{
    [JsonProperty] private float _absorbForce;
    [JsonProperty] private float _forceDelay;

    [JsonIgnore] public float AbsorbForce { get => _absorbForce; set => _absorbForce = value; }
    [JsonIgnore] public float ForceDelay { get => _forceDelay; set => _forceDelay = value; }


    private float _lifetime;
    private float _sizeMultiplier;

    [JsonIgnore] public float Lifetime { get => _lifetime; set => _lifetime = value; }
    [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; set => _sizeMultiplier = value; }


    public BlackholeData(float force, float forceDelay) : base()
    {
        _absorbForce = force;
        _forceDelay = forceDelay;
    }

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

        weapon.InjectData(CopyWeaponData as BlackholeData);
        weapon.Initialize();

        return weapon;
    }
}
