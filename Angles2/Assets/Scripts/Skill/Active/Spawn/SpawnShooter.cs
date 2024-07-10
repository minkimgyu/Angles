using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooter : ActiveSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnShooter(SpawnShooterData data) : base(data._probability)
    {
        _targetTypes = data._targetTypes;
    }

    public override void OnAdd()
    {
        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Shooter);
        if (weapon == null) return;

        weapon.ResetFollower(_castingData.MyTransform);

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
