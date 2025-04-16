using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMode;

[System.Serializable]
public class HealthStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalHealths;
    [JsonProperty] float _additionalHealth;

    public HealthStatModifier() { }

    public HealthStatModifier(List<float> additionalDamageReductions)
    {
        _additionalHealths = additionalDamageReductions;
    }

    public HealthStatModifier(float additionalDamageReduction)
    {
        _additionalHealth = additionalDamageReduction;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.OnMaxHpChanged?.Invoke(_additionalHealths[level]);
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.OnMaxHpChanged?.Invoke(_additionalHealth);
    }
}