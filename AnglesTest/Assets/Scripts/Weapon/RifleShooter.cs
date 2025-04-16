using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : BaseShooter
{
    public override void Initialize(BaseFactory weaponFactory)
    {
        base.Initialize(weaponFactory);
        _actionStrategy = new RifleShooterAttackStrategy(_data, transform, _detectingStrategy.GetTargets, _weaponFactory);
    }
}
