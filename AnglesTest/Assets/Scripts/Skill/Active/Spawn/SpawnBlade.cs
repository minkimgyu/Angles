using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBlade : BaseSkill
{
    SpawnBladeData _data;
    BaseFactory _weaponFactory;

    public SpawnBlade(SpawnBladeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) :base(Type.Active, data._maxUpgradePoint)
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

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        Transform casterTransform = _caster.GetComponent<Transform>();

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
        modifiers.Add(new WeaponSizeModifier(_data._sizeMultiplier));
        modifiers.Add(new WeaponLifetimeModifier(_data._lifetime));

        weapon.ModifyData(modifiers);
        weapon.Activate();
        weapon.ResetPosition(casterTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Vector2 direction = casterTransform.right;
        projectile.Shoot(direction, _data._force);
    }
}