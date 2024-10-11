using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnShooterData : SkillData
{
    public float _damage;
    public float _delay;
    public BaseWeapon.Name _shooterName;
    public BaseWeapon.Name _projectileName;
    public List<ITarget.Type> _targetTypes;

    public SpawnShooterData(int maxUpgradePoint, BaseWeapon.Name shooterName, float damage, float delay, BaseWeapon.Name projectileName, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _shooterName = shooterName;
        _damage = damage;
        _delay = delay;
        _projectileName = projectileName;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new SpawnShooterData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _shooterName,
            _damage,
            _delay,
            _projectileName,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}


public class SpawnShooterCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;
    BaseFactory _weaponFactory;
    ShooterData _data;

    public SpawnShooterCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(data)
    {
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override BaseSkill Create()
    {
        SpawnShooterData data = CopySkillData as SpawnShooterData;
        return new SpawnShooter(data, _upgrader, _weaponFactory);
    }
}