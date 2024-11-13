using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDamageData : SkillData
{
    public DamageRatioStatModifier _damageStatModifier;
    public UpgradeDamageData(int maxUpgradePoint, DamageRatioStatModifier damageStatModifier) : base(maxUpgradePoint) 
    {
        _damageStatModifier = damageStatModifier;
    }

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
