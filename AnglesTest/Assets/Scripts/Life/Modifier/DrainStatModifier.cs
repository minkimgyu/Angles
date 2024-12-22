using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrainStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalDrainRatios;
    [JsonProperty] float _additionalDrainRatio;

    public DrainStatModifier() 
    {
        _additionalDrainRatios = new List<float>();
        _additionalDrainRatio = 0;
    }

    public DrainStatModifier(List<float> additionalDrainRatios)
    {
        _additionalDrainRatios = additionalDrainRatios;
    }

    public DrainStatModifier(float additionalDrainRatio)
    {
        _additionalDrainRatio = additionalDrainRatio;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.DrainRatio += _additionalDrainRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.DrainRatio += _additionalDrainRatio;
    }
}