using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KnockbackData : CooltimeSkillData
{
    public float _damage;
    public float _sizeMultiplier;
    public SerializableVector2 _size;
    public SerializableVector2 _offset;
    public List<ITarget.Type> _targetTypes;

    // + 연산자 오버로딩
    public static KnockbackData operator +(KnockbackData a, KnockbackUpgrader.UpgradableData b)
    {
        return new KnockbackData(
            a._maxUpgradePoint,
            a._coolTime,
            a._maxStackCount,
            a._damage + b.Damage,
            a._sizeMultiplier + b.SizeMultiplier,
            a._size,
            a._offset,
            a._targetTypes
        );
    }

    public KnockbackData(
        int maxUpgradePoint,
        float coolTime,
        int maxStackCount,
        float damage,
        float sizeMultiplier,
        SerializableVector2 size,
        SerializableVector2 offset,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _sizeMultiplier = sizeMultiplier;
        _size = size;
        _offset = offset;
        _targetTypes = targetTypes;
    }
}

public class KnockbackCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public KnockbackCreater(SkillData data, BaseFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        KnockbackData data = _skillData as KnockbackData;
        return new Knockback(data, _effectFactory);
    }
}
