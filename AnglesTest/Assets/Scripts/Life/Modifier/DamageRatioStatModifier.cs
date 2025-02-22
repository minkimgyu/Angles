using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRatioStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalDamageRatios;
    [JsonProperty] float _additionalDamageRatio;

    public DamageRatioStatModifier()
    {
    }

    public DamageRatioStatModifier(List<float> additionalDamageRatios)
    {
        _additionalDamageRatios = additionalDamageRatios;
    }

    public DamageRatioStatModifier(float additionalDamageRatio)
    {
        _additionalDamageRatio = additionalDamageRatio;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.TotalDamageRatio += _additionalDamageRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.TotalDamageRatio += _additionalDamageRatio;
    }
}