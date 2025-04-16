using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;
using Skill.Strategy;

public class SpawnShooter : BaseSkill
{
    BaseWeapon _weapon;
    BaseFactory _weaponFactory;
    SpawnShooterData _data;

    // 여기서 슈터 데이터와 무기 데이터를 같이 받아오기
    public SpawnShooter(SpawnShooterData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader; // 이건 생성자에서 받아서 쓰기
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.GroggyDuration
        );

        ShooterDataModifier shooterDataModifier = new ShooterDataModifier(damageData, _data.Delay, _data.TargetTypes);
        _weapon.ModifyData(shooterDataModifier);
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _actionStrategy = new SpawnShooterStrategy(
            _caster,
            _upgradeableRatio,
            _data.ShooterName,
            _data.AdRatio,
            _data.Damage,
            _data.Delay,
            _data.GroggyDuration,
            _data.TargetTypes,
            _weaponFactory);
        // _actionStrategy = 폭탄 스폰 기능 추가
    }

    public override void OnAdd()
    {
        IFollowable followable = _caster.GetComponent<IFollowable>();   
        if (followable == null) return;

        _weapon = _actionStrategy.Execute(followable, new SpawnShooterStrategy.ChangeableData(_data.Damage, _data.Delay));
    }
}