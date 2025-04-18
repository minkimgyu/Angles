using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooter : BaseShooter
{
    public override void Initialize(BaseFactory weaponFactory)
    {
        base.Initialize(weaponFactory);
        _actionStrategy = new RocketShooterAttackStrategy(_data, transform, _detectingStrategy.GetTargets, _weaponFactory);
    }
}
