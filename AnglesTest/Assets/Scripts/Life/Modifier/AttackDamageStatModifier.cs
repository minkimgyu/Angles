using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDamageStatModifier : IStatModifier
{
    [JsonProperty] List<float> _additionalAttackDamages;
    [JsonProperty] float _additionalAttackDamage;

    public AttackDamageStatModifier() { }

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