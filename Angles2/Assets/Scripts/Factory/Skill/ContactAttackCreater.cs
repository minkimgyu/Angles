using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ContactAttackData : BaseSkillData
{
    public float _damage;
    public List<ITarget.Type> _targetTypes;

    public ContactAttackData(int maxUpgradePoint, float damage, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _targetTypes = targetTypes;
    }
}

public class ContactAttackCreater : SkillCreater
{
    Func<BaseEffect.Name, BaseEffect> CreateEffect;

    public ContactAttackCreater(BaseSkillData data, Func<BaseEffect.Name, BaseEffect> CreateEffect) : base(data)
    {
        this.CreateEffect = CreateEffect;
    }

    public override BaseSkill Create()
    {
        ContactAttackData data = _skillData as ContactAttackData;
        return new ContactAttack(data, CreateEffect);
    }
}