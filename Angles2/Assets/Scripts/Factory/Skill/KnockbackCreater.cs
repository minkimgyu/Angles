using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KnockbackData : CooltimeSkillData
{
    public float _damage;
    public float _adRatio;
    public float _rangeMultiplier;
    public SerializableVector2 _size;
    public SerializableVector2 _offset;
    public List<ITarget.Type> _targetTypes;
    public float _groggyDuration;

    public KnockbackData(
        int maxUpgradePoint,
        float coolTime,
        int maxStackCount,
        float damage,
        float adRatio,
        float groggyDuration,
        SerializableVector2 size,
        SerializableVector2 offset,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _rangeMultiplier = 1;
        _size = size;
        _offset = offset;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new KnockbackData(
            _maxUpgradePoint, // CooltimeSkillData에서 상속된 값
            _coolTime, // CooltimeSkillData에서 상속된 값
            _maxStackCount, // CooltimeSkillData에서 상속된 값
            _damage,
            _adRatio,
            _groggyDuration,
            new SerializableVector2(_size.x, _size.y), // SerializableVector2 깊은 복사
            new SerializableVector2(_offset.x, _offset.y), // SerializableVector2 깊은 복사
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class KnockbackCreater : SkillCreater
{
    BaseFactory _effectFactory;
    IUpgradeVisitor _upgrader;

    public KnockbackCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        KnockbackData data = CopySkillData as KnockbackData;
        return new Knockback(data, _upgrader, _effectFactory);
    }
}
