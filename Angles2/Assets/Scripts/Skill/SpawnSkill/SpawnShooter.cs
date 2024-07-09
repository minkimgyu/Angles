using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooter : ActiveSkill
{
    List<ITarget.Type> _targetType;

    public SpawnShooter(float probability, List<ITarget.Type> damageableTypes)
    {
        _probability = probability;
        _targetType = damageableTypes;
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Shooter);
        if (weapon == null) return;

        weapon.ResetFollower(_castingData.MyTransform);

        weapon.ResetDamageableTypes(_targetType);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
