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
    BaseFactory _effectFactory;

    public ContactAttackCreater(BaseSkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ContactAttackData data = _buffData as ContactAttackData;
        return new ContactAttack(data, _effectFactory);
    }
}