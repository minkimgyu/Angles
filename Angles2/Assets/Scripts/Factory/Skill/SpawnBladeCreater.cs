using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnBladeData : RandomSkillData
{
    public float _damage;
    public float _lifetime;
    public float _sizeMultiplier;

    public List<ITarget.Type> _targetTypes;
    public float _force;

    public SpawnBladeData(int maxUpgradePoint, float probability, float damage, float lifetime, float force, List<ITarget.Type> targetTypes) : base(maxUpgradePoint, probability)
    {
        _damage = damage;
        _lifetime = lifetime;
        _sizeMultiplier = 1;

        _force = force;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnBladeData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _probability, // RandomSkillData에서 상속된 값
            _damage,
            _lifetime,
            _force,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class SpawnBladeCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;

    public SpawnBladeCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnBladeData data = _skillData as SpawnBladeData;
        return new SpawnBlade(data, _upgrader, _weaponFactory);
    }
}
