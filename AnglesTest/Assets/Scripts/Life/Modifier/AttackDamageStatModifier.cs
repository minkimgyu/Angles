using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatModifier : IStatModifier
{
    List<float> _additionalAttackDamages;
    float _additionalAttackDamage;

    public AttackDamageStatModifier(List<float> additionalAttackDamages)
    {
        _additionalAttackDamages = additionalAttackDamages;
    }

    public AttackDamageStatModifier(float additionalAttackDamage)
    {
        _additionalAttackDamage = additionalAttackDamage;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.AttackDamage += _additionalAttackDamages[level];
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.AttackDamage += _additionalAttackDamage;
    }
}