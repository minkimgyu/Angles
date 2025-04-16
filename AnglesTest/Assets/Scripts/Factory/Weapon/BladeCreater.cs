using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class BladeData : WeaponData
{
    [JsonProperty] private float _attackDelay;
    [JsonProperty] private float _groggyDuration;

    [JsonIgnore] public float AttackDelay { get => _attackDelay; set => _attackDelay = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }


    [JsonProperty] float _lifetime;
    [JsonProperty] float _sizeMultiplier;

    [JsonIgnore] public float Lifetime { get => _lifetime; set => _lifetime = value; }
    [JsonIgnore] public float SizeMultiplier { get => _sizeMultiplier; set => _sizeMultiplier = value; }


    public BladeData(float attackDelay, float groggyDuration)
    {
        _attackDelay = attackDelay;
        _groggyDuration = groggyDuration;
    }

    public override WeaponData Copy()
    {
        return new BladeData(
            _attackDelay,
            _groggyDuration
        );
    }
}

public class BladeCreater : WeaponCreater
{
    public BladeCreater(BaseWeapon weaponPrefab, WeaponData data) : base(weaponPrefab, data)
    {
    }

    public override BaseWeapon Create()
    {
        BaseWeapon weapon = Object.Instantiate(_weaponPrefab);
        if (weapon == null) return null;

        weapon.InjectData(CopyWeaponData as BladeData);
        weapon.Initialize();
        return weapon;
    }
}
