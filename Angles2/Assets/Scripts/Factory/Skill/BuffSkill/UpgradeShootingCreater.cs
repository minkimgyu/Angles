using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShootingData : SkillData
{
    public List<StatUpgrader.ShootingData> _shootingDatas;
    public UpgradeShootingData(int maxUpgradePoint, List<StatUpgrader.ShootingData> shootingDatas) : base(maxUpgradePoint)
    {
        _shootingDatas = shootingDatas;
    }

    public override SkillData Copy()
    {
        return new UpgradeShootingData(_maxUpgradePoint, _shootingDatas);
    }
}

public class UpgradeShootingCreater : SkillCreater
{

    public UpgradeShootingCreater(SkillData data) : base(data)
    {
    }

    public override BaseSkill Create()
    {
        UpgradeShootingData data = CopySkillData as UpgradeShootingData;
        return new UpgradeShooting(data);
    }
}