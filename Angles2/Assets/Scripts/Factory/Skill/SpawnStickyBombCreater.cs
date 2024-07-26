using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnStickyBombData : CooltimeSkillData
{
    public int maxStackCount;
    public List<ITarget.Type> _targetTypes;

    public SpawnStickyBombData(int maxUpgradePoint, float coolTime, int maxStackCount, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _targetTypes = targetTypes;
    }
}

public class SpawnStickyBombCreater : SkillCreater
{
    public override BaseSkill Create()
    {
        SpawnStickyBombData data = Database.Instance.SkillDatas[BaseSkill.Name.SpawnStickyBomb] as SpawnStickyBombData;
        return new SpawnStickyBomb(data);
    }
}
