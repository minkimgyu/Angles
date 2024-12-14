using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooltimeStatModifier : IStatModifier
{
    List<float> _additionalCooltimeRatios;
    float _additionalCooltimeRatio;

    public CooltimeStatModifier(List<float> additionalCooltimeRatios)
    {
        _additionalCooltimeRatios = additionalCooltimeRatios;
    }

    public CooltimeStatModifier(float additionalCooltimeRatio)
    {
        _additionalCooltimeRatio = additionalCooltimeRatio;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.TotalCooltimeRatio += _additionalCooltimeRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.TotalCooltimeRatio += _additionalCooltimeRatio;
    }
}