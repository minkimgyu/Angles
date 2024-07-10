using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    protected CastingData _castingData;
    protected float _probability;

    public override void Initialize(CastingData data)
    {
        _castingData = data;
    }

    public ActiveSkill(float probability)
    {
        _probability = probability;
    }

    public override bool CanUse()
    {
        float random = Random.Range(0.0f, 1.1f);
        return random <= _probability;
    }
}
