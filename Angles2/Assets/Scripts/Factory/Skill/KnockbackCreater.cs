using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct KnockbackUpgradableData
{
    public KnockbackUpgradableData(float damage, float sizeMultiplier)
    {
        _damage = damage;
        _size = sizeMultiplier;
    }

    private float _damage;
    private float _size;

    public float Damage { get => _damage; }
    public float Size { get => _size; }
}

[Serializable]
public class KnockbackData : CooltimeSkillData
{
    public List<KnockbackUpgradableData> _upgradableDatas;
    public SerializableVector2 _size;
    public SerializableVector2 _offset;
    public List<ITarget.Type> _targetTypes;

    public KnockbackData(int maxUpgradePoint, float coolTime, int maxStackCount, List<KnockbackUpgradableData> upgradableDatas, SerializableVector2 size, SerializableVector2 offset, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _upgradableDatas = upgradableDatas;
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
