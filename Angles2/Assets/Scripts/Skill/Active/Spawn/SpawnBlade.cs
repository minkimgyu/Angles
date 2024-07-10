using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlade : ActiveSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnBlade(SpawnBladeData data) :base(data._probability)
    {
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);

        IProjectile projectile = weapon.GetComponent<IProjectile>();
        if (projectile == null) return;

        Vector2 direction = _castingData.MyTransform.right;
        projectile.Shoot(direction);
    }
}
