using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnShooterData : BaseSkillData
{
    public List<ITarget.Type> _targetTypes;

    public SpawnShooterData(int maxUpgradePoint, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _targetTypes = targetTypes;
    }
}


public class SpawnShooterCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SpawnShooterData data = Database.Instance.SkillDatas[BaseSkill.Name.SpawnShooter] as SpawnShooterData;
        return new SpawnShooter(data);
    }
}
