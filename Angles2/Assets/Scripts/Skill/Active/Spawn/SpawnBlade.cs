using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBlade : RandomSkill
{
    List<ITarget.Type> _targetTypes;
    public float _force;
    Func<BaseWeapon.Name, BaseWeapon> CreateWeapon;

    BladeData _bladeData;

    public SpawnBlade(SpawnBladeData data, Func<BaseWeapon.Name, BaseWeapon> CreateWeapon) :base(data._maxUpgradePoint, data._probability)
    {
        _bladeData = data._data;
        _targetTypes = data._targetTypes;
        _force = data._force;

        this.CreateWeapon = CreateWeapon;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = CreateWeapon?.Invoke(BaseWeapon.Name.Blade);
        if (weapon == null) return;


        weapon.ResetData(_bladeData);    
        weapon.Upgrade(_upgradePoint); // 생성 후 업그레이드 적용
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Vector2 direction = _castingData.MyTransform.right;
        projectile.Shoot(direction, _force);
    }
}
