using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatikkData : CooltimeSkillData
{
    public float _damage;
    public float _range;
    public int _maxTargetCount;
    public List<ITarget.Type> _targetTypes;

    public StatikkData(int maxUpgradePoint, float coolTime, int maxStackCount, float damage, float range, int maxTargetCount, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
        _maxTargetCount = maxTargetCount;
    }
}

public class StatikkCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        StatikkData data = Database.Instance.SkillDatas[BaseSkill.Name.Statikk] as StatikkData;
        return new Statikk(data);
    }
}
