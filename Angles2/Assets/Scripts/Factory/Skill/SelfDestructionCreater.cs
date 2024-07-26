using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelfDestructionData : BaseSkillData
{
    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public SelfDestructionData(int maxUpgradePoint, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _delay = delay;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class SelfDestructionCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SelfDestructionData data = Database.Instance.SkillDatas[BaseSkill.Name.SelfDestruction] as SelfDestructionData;
        return new SelfDestruction(data);
    }
}