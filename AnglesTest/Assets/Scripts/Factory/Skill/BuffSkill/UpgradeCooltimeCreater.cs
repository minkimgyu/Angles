using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeCooltimeData : SkillData
{
    [JsonProperty] private CooltimeStatModifier _cooltimeStatModifier;
    public UpgradeCooltimeData(int maxUpgradePoint, CooltimeStatModifier cooltimeStatModifier) : base(maxUpgradePoint)
    {
        _cooltimeStatModifier = cooltimeStatModifier;
    }

    [JsonIgnore] public CooltimeStatModifier CooltimeStatModifier { get => _cooltimeStatModifier; set => _cooltimeStatModifier = value; }

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