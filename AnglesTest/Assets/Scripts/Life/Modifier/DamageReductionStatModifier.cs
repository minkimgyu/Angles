using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageReductionStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalDamageReductionRatios;
    [JsonProperty] float _additionalDamageReductionRatio;

    public DamageReductionStatModifier()
    {
        _additionalDamageReductionRatios = new List<float>();
        _additionalDamageReductionRatio = 0;
    }

    public DamageReductionStatModifier(List<float> additionalDamageReductionRatios)
    {
        _additionalDamageReductionRatios = additionalDamageReductionRatios;
    }

    public DamageReductionStatModifier(float additionalDamageReductionRatio)
    {
        _additionalDamageReductionRatio = additionalDamageReductionRatio;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.DamageReductionRatio.Value += _additionalDamageReductionRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.DamageReductionRatio.Value += _additionalDamageReductionRatio;
    }
}