using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            _data.TargetTypes,
            _data.GroggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data.Delay));
        _weapon.ModifyData(modifiers);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(_data.ShooterName);
        if (weapon == null) return;

        IFollowable followable = _caster.GetComponent<IFollowable>();
        if (followable == null) return;

        _weapon = weapon;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data.Delay));
        modifiers.Add(new WeaponProjectileModifier(_data.ProjectileName));

        _weapon.ModifyData(modifiers);
        _weapon.Activate();

        Transform casterTransform = _caster.GetComponent<Transform>();
        _weapon.ResetFollower(followable);
        _weapon.ResetPosition(casterTransform.position);
    }
}