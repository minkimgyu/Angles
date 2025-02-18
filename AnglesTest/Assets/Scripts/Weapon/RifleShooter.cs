using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : BaseShooter
{
    public override void Initialize(BaseFactory weaponFactory)
    {
        base.Initialize(weaponFactory);
        _attackStrategy = new RifleShooterAttackStrategy(_data, transform, _weaponFactory);
    }
}
