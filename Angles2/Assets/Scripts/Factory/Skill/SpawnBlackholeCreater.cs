using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBlackholeData : RandomSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnBlackholeData(int maxUpgradePoint, float probability, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnBlackholeCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SpawnBlackholeData data = Database.Instance.SkillDatas[BaseSkill.Name.SpawnBlackhole] as SpawnBlackholeData;
        return new SpawnBlackhole(data);
    }
}
