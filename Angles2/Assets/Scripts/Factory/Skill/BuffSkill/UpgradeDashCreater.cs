using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDashData : SkillData
{
    public List<StatUpgrader.DashData> _dashDatas;
    public UpgradeDashData(int maxUpgradePoint, List<StatUpgrader.DashData> dashDatas) : base(maxUpgradePoint)
    {
        _dashDatas = dashDatas;
    }

    public override SkillData Copy()
    {
        return new UpgradeDashData(_maxUpgradePoint, _dashDatas);
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