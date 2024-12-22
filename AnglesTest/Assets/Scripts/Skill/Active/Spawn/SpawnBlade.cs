using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBlade : BaseSkill
{
    SpawnBladeData _data;
    BaseFactory _weaponFactory;

    public SpawnBlade(SpawnBladeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) :base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new RandomConstraintComponent(_data, _upgradeableRatio);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void OnReflect(GameObject targetObject, Vector3 contactPos)
    {
        ITarget target = targetObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        Transform casterTransform = _caster.GetComponent<Transform>();

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
        modifiers.Add(new WeaponSizeModifier(_data.SizeMultiplier));
        modifiers.Add(new WeaponLifetimeModifier(_data.Lifetime));

        weapon.ModifyData(modifiers);
        weapon.Activate();
        weapon.ResetPosition(casterTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Vector2 direction = casterTransform.right;
        projectile.Shoot(direction, _data.Force);
    }
}
