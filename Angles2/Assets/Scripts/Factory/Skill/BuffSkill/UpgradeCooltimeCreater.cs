using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCooltimeData : SkillData
{
    public CooltimeStatModifier _cooltimeStatModifier;
    public UpgradeCooltimeData(int maxUpgradePoint, CooltimeStatModifier cooltimeStatModifier) : base(maxUpgradePoint)
    {
        _cooltimeStatModifier = cooltimeStatModifier;
    }

    public override SkillData Copy()
    {
        return new UpgradeCooltimeData(_maxUpgradePoint, _cooltimeStatModifier);
    }
}

public class UpgradeCooltimeCreater : SkillCreater
{
    public UpgradeCooltimeCreater(SkillData data) : base(data) { }

    public override BaseSkill Create()
    {
        UpgradeCooltimeData data = CopySkillData as UpgradeCooltimeData;
        return new UpgradeCooltime(data);
    }
}