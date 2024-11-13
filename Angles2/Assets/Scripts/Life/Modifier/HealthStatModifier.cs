using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStatModifier : IStatModifier
{
    List<float> _additionalDamageReductions;
    float _additionalDamageReduction;

    public HealthStatModifier(List<float> additionalDamageReductions)
    {
        _additionalDamageReductions = additionalDamageReductions;
    }

    public HealthStatModifier(float additionalDamageReduction)
    {
        _additionalDamageReduction = additionalDamageReduction;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.MaxHp += _additionalDamageReductions[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.MaxHp += _additionalDamageReduction;
    }
}