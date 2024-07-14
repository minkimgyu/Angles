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

        IFollowable followable = _castingData.MyObject.GetComponent<IFollowable>();
        if (followable == null) return;

        weapon.ResetFollower(followable);

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
