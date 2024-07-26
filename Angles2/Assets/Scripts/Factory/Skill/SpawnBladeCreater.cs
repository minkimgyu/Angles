using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnBladeData : RandomSkillData
{
    public List<ITarget.Type> _targetTypes;
    public float _force;

    public SpawnBladeData(int maxUpgradePoint, float probability, float force, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _targetTypes = targetTypes;
        _force = force;
    }
}

public class SpawnBladeCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SpawnBladeData data = Database.Instance.SkillDatas[BaseSkill.Name.SpawnBlade] as SpawnBladeData;
        return new SpawnBlade(data);
    }
}
