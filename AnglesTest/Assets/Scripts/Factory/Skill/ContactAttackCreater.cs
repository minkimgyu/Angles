using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ContactAttackData : SkillData
{
    public float _damage;
    public float _adRatio;
    public float _groggyDuration;
    public List<ITarget.Type> _targetTypes;

    public ContactAttackData(int maxUpgradePoint, float damage, float adRatio, float groggyDuration, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
       return new ContactAttackData(
           _maxUpgradePoint,
           _damage,
           _adRatio,
           _groggyDuration,
           _targetTypes
       );
    }
}

public class ContactAttackCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ContactAttackCreater(SkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ContactAttackData data = CopySkillData as ContactAttackData;
        return new ContactAttack(data, _effectFactory);
    }
}