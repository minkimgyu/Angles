using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public override bool CanUse()
    {
        float random = Random.Range(0.0f, 1.1f);
        return random <= _probability;
    }
}
