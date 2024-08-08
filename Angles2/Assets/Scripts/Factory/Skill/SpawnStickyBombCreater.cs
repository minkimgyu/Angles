using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
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
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    public SpawnStickyBombCreater(BaseSkillData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) : base(data)
    {
        this.CreateWeapon = CreateWeapon;
    }

    public override BaseSkill Create()
    {
        SpawnStickyBombData data = _skillData as SpawnStickyBombData;
        return new SpawnStickyBomb(data, CreateWeapon);
    }
}
