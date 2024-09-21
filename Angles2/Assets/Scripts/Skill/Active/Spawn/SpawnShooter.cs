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
    public SpawnShooter(SpawnShooterData data, BaseFactory weaponFactory) : base(Type.Passive, data._maxUpgradePoint)
    {
        _data = data;
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(_data._damage));
        modifiers.Add(new WeaponDelayModifier(_data._delay));
        _weapon.ModifyData(modifiers);
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = _weaponFactory.Create(_data._shooterName);
        if (weapon == null) return;

        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
        if (followable == null) return;

        _weapon = weapon;

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(_data._damage));
        modifiers.Add(new WeaponDamageModifier(_data._delay));
        modifiers.Add(new WeaponProjectileModifier(_data._projectileName));
        modifiers.Add(new WeaponTargetModifier(_data._targetTypes));

        _weapon.ModifyData(modifiers);

        _weapon.ResetFollower(followable);
        _weapon.ResetPosition(_castingData.MyTransform.position);
    }
}