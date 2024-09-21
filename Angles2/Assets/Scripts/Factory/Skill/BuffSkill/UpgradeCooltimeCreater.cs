using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCooltimeData : SkillData
{
    public List<StatUpgrader.CooltimeData> _cooltimeDatas;
    public UpgradeCooltimeData(int maxUpgradePoint, List<StatUpgrader.CooltimeData> cooltimeDatas) : base(maxUpgradePoint)
    {
        _cooltimeDatas = cooltimeDatas;
    }

    public override SkillData Copy()
    {
        return new UpgradeCooltimeData(_maxUpgradePoint, _cooltimeDatas);
    }
}

public class UpgradeCooltimeCreater : SkillCreater
{
    public UpgradeCooltimeCreater(SkillData data) : base(data) { }

    public override BaseSkill Create()
    {
        UpgradeCooltimeData data = _skillData as UpgradeCooltimeData;
        return new UpgradeCooltime(data);
    }
}