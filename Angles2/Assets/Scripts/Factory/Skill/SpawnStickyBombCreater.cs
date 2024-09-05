using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnStickyBombData : CooltimeSkillData
{
    public int maxStackCount;
    public List<ITarget.Type> _targetTypes;
    public StickyBombData _data;

    public SpawnStickyBombData(int maxUpgradePoint, float coolTime, int maxStackCount, StickyBombData data, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _targetTypes = targetTypes;
        _data = data;
    }
}

public class SpawnStickyBombCreater : SkillCreater
{
    BaseFactory _weaponFactory;

    public SpawnStickyBombCreater(BaseSkillData data, BaseFactory _weaponFactory) : base(data)
    {
        this._weaponFactory = _weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnStickyBombData data = _buffData as SpawnStickyBombData;
        return new SpawnStickyBomb(data, _weaponFactory);
    }
}
