using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShockwaveData : SkillData
{
    public float _damage;
    public float _delay;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public ShockwaveData(int maxUpgradePoint, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _delay = delay;
        _range = range;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ShockwaveData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _damage,
            _range,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class ShockwaveCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public ShockwaveCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ShockwaveData data = _skillData as ShockwaveData;
        return new Shockwave(data, _effectFactory);
    }
}