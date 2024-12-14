using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainStatModifier : IStatModifier
{
    List<float> _additionalDrainRatios;
    float _additionalDrainRatio;

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
        data._drainRatio += _additionalDrainRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data._drainRatio += _additionalDrainRatio;
    }
}