using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : ActiveSkill
{
    List<ITarget.Type> _targetType;

    public SpawnBlackhole(float probability, List<ITarget.Type> damageableTypes)
    {
        _probability = probability;
        _targetType = damageableTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        weapon.ResetDamageableTypes(_targetType);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
