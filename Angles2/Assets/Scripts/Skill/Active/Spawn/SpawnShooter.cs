using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnShooter : BaseSkill
{
    BaseWeapon _weapon;
    BaseFactory _weaponFactory;
    SpawnShooterData _data;

    // ���⼭ ���� �����Ϳ� ���� �����͸� ���� �޾ƿ���
    public SpawnShooter(SpawnShooterData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader; // �̰� �����ڿ��� �޾Ƽ� ����
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
                _data._damage,
                _upgradeableRatio.AttackDamage,
                _data._adRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data._targetTypes,
            _data._groggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data._delay));
        _weapon.ModifyData(modifiers);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(_data._shooterName);
        if (weapon == null) return;

        IFollowable followable = _caster.GetComponent<IFollowable>();
        if (followable == null) return;

        _weapon = weapon;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data._damage,
                _upgradeableRatio.AttackDamage,
                _data._adRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data._targetTypes,
            _data._groggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));
        modifiers.Add(new WeaponDelayModifier(_data._delay));
        modifiers.Add(new WeaponProjectileModifier(_data._projectileName));

        _weapon.ModifyData(modifiers);
        _weapon.Activate();

        Transform casterTransform = _caster.GetComponent<Transform>();
        _weapon.ResetFollower(followable);
        _weapon.ResetPosition(casterTransform.position);
    }
}