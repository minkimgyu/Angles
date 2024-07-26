using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : BaseSkill
{
    protected float _probability;
    public RandomSkill(int maxUpgradePoint, float probability) : base(Type.Active, maxUpgradePoint)
    {
        _probability = probability;
    }

    public override bool CanUse()
    {
        float random = Random.Range(0.0f, 1.0f);
        return random <= _probability;
    }
}
