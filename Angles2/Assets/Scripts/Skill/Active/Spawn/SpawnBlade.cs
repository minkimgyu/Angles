using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlade : ActiveSkill
{
    List<ITarget.Type> _targetTypes;
    public float _force;

    public SpawnBlade(SpawnBladeData data) :base(data._probability)
    {
        _targetTypes = data._targetTypes;
        _force = data._force;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Vector2 direction = _castingData.MyTransform.right;
        projectile.Shoot(direction, _force);
    }
}
