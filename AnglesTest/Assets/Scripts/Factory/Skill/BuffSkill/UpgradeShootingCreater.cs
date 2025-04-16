using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

[System.Serializable]
public class UpgradeShootingData : SkillData
{
    [JsonProperty] private ShootingStatModifier _shootingStatModifier;

    public UpgradeShootingData(int maxUpgradePoint, ShootingStatModifier shootingStatModifier) : base(maxUpgradePoint)
    {
        _shootingStatModifier = shootingStatModifier;
    }

    [JsonIgnore] public ShootingStatModifier ShootingStatModifier { get => _shootingStatModifier; set => _shootingStatModifier = value; }

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