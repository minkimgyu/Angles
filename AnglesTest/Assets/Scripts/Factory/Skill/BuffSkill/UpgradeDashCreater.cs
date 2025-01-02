using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDashData : SkillData
{
    public DashStatModifier _dashStatModifier;
    public UpgradeDashData(int maxUpgradePoint, DashStatModifier dashStatModifier) : base(maxUpgradePoint)
    {
        _dashStatModifier = dashStatModifier;
    }

    public override SkillData Copy()
    {
        return new UpgradeDashData(_maxUpgradePoint, _dashStatModifier);
    }
}

public class UpgradeDashCreater : SkillCreater
{
    public UpgradeDashCreater(SkillData data) : base(data)
    {
    }

    public override BaseSkill Create()
    {
        UpgradeDashData data = CopySkillData as UpgradeDashData;
        return new UpgradeDash(data);
    }
}