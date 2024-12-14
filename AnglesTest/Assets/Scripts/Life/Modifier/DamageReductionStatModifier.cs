using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReductionStatModifier : IStatModifier
{
    List<float> _additionalDamageReductionRatios;
    float _additionalDamageReductionRatio;

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
        data._damageReductionRatio += _additionalDamageReductionRatios[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data._damageReductionRatio += _additionalDamageReductionRatio;
    }
}