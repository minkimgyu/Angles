using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactData : RandomSkillData
{
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ImpactData(int maxUpgradePoint, float probability, float damage, float range, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}

public class ImpactCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        ImpactData data = Database.Instance.SkillDatas[BaseSkill.Name.Impact] as ImpactData;
        return new Impact(data);
    }
}
