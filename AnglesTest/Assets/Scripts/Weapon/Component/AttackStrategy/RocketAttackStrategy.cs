using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RocketAttackStrategy : IWeaponActionStrategy
{
    RocketData _rocketData;
    Action OnHit;

    public RocketAttackStrategy(RocketData rocketData, Action OnHit)
    {
        _rocketData = rocketData;
        this.OnHit = OnHit;
    }

    public void OnTargetEnter(Collider2D collider)
    {
        ITarget target = collider.GetComponent<ITarget>();
        if (target == null) // 벽의 경우
        {
            OnHit?.Invoke();
            return;
        }

        if (target.IsTarget(_rocketData.DamageableData._targetType) == true)
        {
            OnHit?.Invoke();
            return;
        }
    }
}
