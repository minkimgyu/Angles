using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeDamageData : SkillData
{
    [JsonProperty] private DamageRatioStatModifier _damageStatModifier;
    public UpgradeDamageData(int maxUpgradePoint, DamageRatioStatModifier damageStatModifier) : base(maxUpgradePoint) 
    {
        _damageStatModifier = damageStatModifier;
    }

    [JsonIgnore] public DamageRatioStatModifier DamageStatModifier { get => _damageStatModifier; set => _damageStatModifier = value; }

    public override SkillData Copy()
    {
        return new UpgradeDamageData(_maxUpgradePoint, _damageStatModifier);
    }
}

public class UpgradeDamageCreater : SkillCreater
{
    public UpgradeDamageCreater(SkillData data) : base(data) { }

    public override BaseSkill Create()
    {
        UpgradeDamageData data = CopySkillData as UpgradeDamageData;
        return new UpgradeDamage(data);
    }
}
