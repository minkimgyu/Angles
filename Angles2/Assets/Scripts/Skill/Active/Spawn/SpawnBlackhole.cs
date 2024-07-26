using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlackhole : RandomSkill
{
    List<ITarget.Type> _targetTypes;

    public SpawnBlackhole(SpawnBlackholeData data) : base(data._maxUpgradePoint, data._probability)
    {
        _targetTypes = data._targetTypes;
    }

    public override void OnReflect(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        BaseWeapon weapon = WeaponFactory.Create(BaseWeapon.Name.Blackhole);
        if (weapon == null) return;

        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(_castingData.MyTransform.position);
    }
}
