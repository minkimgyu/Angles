using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    public override BaseSkill Create()
    {
        ContactAttackData data = Database.Instance.SkillDatas[BaseSkill.Name.ContactAttack] as ContactAttackData;
        return new ContactAttack(data);
    }
}