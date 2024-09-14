using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBlade : BaseSkill
{
    SpawnBladeData _data;
    BladeData _bladeData;
    BaseFactory _weaponFactory;

    public SpawnBlade(SpawnBladeData data, SpawnBladeUpgrader upgrader, BaseFactory weaponFactory) :base(Type.Active, data._maxUpgradePoint)
    {
        _data = data;
        _upgradeVisitor = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        weapon.ResetData(_bladeData);    
        weapon.ResetTargetTypes(_data._targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Vector2 direction = _castingData.MyTransform.right;
        projectile.Shoot(direction, _data._force);
    }
}
