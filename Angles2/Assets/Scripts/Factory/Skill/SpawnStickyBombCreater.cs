using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnStickyBombData : CooltimeSkillData
{
    public List<ITarget.Type> _targetTypes;
    public float _damage;
    public float _delay;

    public SpawnStickyBombData(int maxUpgradePoint, float coolTime, int maxStackCount, float damage, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _maxStackCount = maxStackCount;
        _damage = damage;
        _delay = delay;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnStickyBombData(
            _maxUpgradePoint, // CooltimeSkillData���� ��ӵ� ��
            _coolTime, // CooltimeSkillData���� ��ӵ� ��
            _maxStackCount, // CooltimeSkillData���� ��ӵ� ��
            _damage,
            _delay,
            new List<ITarget.Type>(_targetTypes) // ����Ʈ ���� ����
        );
    }
}

public class SpawnStickyBombCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;

    public SpawnStickyBombCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnStickyBombData data = CopySkillData as SpawnStickyBombData;
        return new SpawnStickyBomb(data, _upgrader, _weaponFactory);
    }
}
