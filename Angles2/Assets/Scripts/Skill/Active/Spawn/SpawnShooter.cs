using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShooter : BaseSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnShooter(SpawnShooterData data) : base(Type.Passive, data._maxUpgradePoint)
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
