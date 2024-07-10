using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : ActiveSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnBlackhole(SpawnBlackholeData data) : base(data._probability)
    {
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Blade);
        if (weapon == null) return;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
