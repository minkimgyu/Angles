using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShootingData : SkillData
{
    public ShootingStatModifier _shootingStatModifier;

    public UpgradeShootingData(int maxUpgradePoint, ShootingStatModifier shootingStatModifier) : base(maxUpgradePoint)
    {
        _shootingStatModifier = shootingStatModifier;
    }

    public override SkillData Copy()
    {
        return new UpgradeShootingData(_maxUpgradePoint, _shootingStatModifier);
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