using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnBlackholeData : RandomSkillData
{
    public float _damage;
    public float _adRatio;
    public float _groggyDuration;

    public float _sizeMultiplier;
    public float _lifetime;

    public List<ITarget.Type> _targetTypes;

    public SpawnBlackholeData(
        int maxUpgradePoint,
        float probability,

        float damage,
        float adRatio,
        float groggyDuration,

        float lifetime,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _sizeMultiplier = 1;
        _lifetime = lifetime;

        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;

        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnBlackholeData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값

            _damage,
            _adRatio,
            _groggyDuration,

            _lifetime,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SpawnBlackholeCreater : SkillCreater
{
    BaseFactory _weaponFactory;
    IUpgradeVisitor _upgrader;

    public SpawnBlackholeCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnBlackholeData data = CopySkillData as SpawnBlackholeData;
        return new SpawnBlackhole(data, _upgrader, _weaponFactory);
    }
}
