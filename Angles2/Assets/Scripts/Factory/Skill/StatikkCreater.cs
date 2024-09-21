using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;


// ���׷��̵� ������ ��ų �����͸� ���� Struct�� ���� ��������
[Serializable]
public class StatikkData : CooltimeSkillData
{
    public float _damage;
    public float _range;
    public int _maxTargetCount;
    public List<ITarget.Type> _targetTypes;

    public StatikkData(
        int maxUpgradePoint,
        float coolTime,
        int maxStackCount,
        float damage,
        float range,
        int maxTargetCount,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _range = range;
        _maxTargetCount = maxTargetCount;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new StatikkData(
            _maxUpgradePoint, // CooltimeSkillData���� ��ӵ� ��
            _coolTime, // CooltimeSkillData���� ��ӵ� ��
            _maxStackCount, // CooltimeSkillData���� ��ӵ� ��
            _damage,
            _range,
            _maxTargetCount,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ ���� ����
        );
    }
}

public class StatikkCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _effectFactory;

    public StatikkCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data) 
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        StatikkData data = _skillData as StatikkData;
        return new Statikk(data, _upgrader, _effectFactory);
    }
}
