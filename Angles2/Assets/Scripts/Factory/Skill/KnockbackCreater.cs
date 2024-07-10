using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnockbackData : BaseSkillData
{
    public float _damage;
    public SerializableVector2 _size;
    public SerializableVector2 _offset;
    public List<ITarget.Type> _targetType;

    public KnockbackData(float probability, float damage, SerializableVector2 size, SerializableVector2 offset, List<ITarget.Type> targetTypes) : base(probability)
    {
        _damage = damage;
        _size = size;
        _offset = offset;
        _targetType = targetTypes;
    }
}

public class KnockbackCreater : SkillCreater<KnockbackData>
{
    public override BaseSkill Create()
    {
        return new Knockback(_data);
    }
}
