using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KnockbackData : CooltimeSkillData
{
    public float _damage;
    public SerializableVector2 _size;
    public SerializableVector2 _offset;
    public List<ITarget.Type> _targetTypes;

    public KnockbackData(int maxUpgradePoint, float coolTime, int maxStackCount, float damage, SerializableVector2 size, SerializableVector2 offset, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _size = size;
        _offset = offset;
        _targetTypes = targetTypes;
    }
}

public class KnockbackCreater : SkillCreater
{
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    public KnockbackCreater(BaseSkillData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data)
    {
        this.CreateEffect = CreateEffect;
    }

    public override BaseSkill Create()
    {
        KnockbackData data = _skillData as KnockbackData;
        return new Knockback(data, CreateEffect);
    }
}
