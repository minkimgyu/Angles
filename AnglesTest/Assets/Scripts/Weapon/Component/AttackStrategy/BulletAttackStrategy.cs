using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class BulletAttackStrategy : IAttackStrategy
{
    IAttackStat _attackStat;
    Action OnHit;

    public BulletAttackStrategy(IAttackStat attackStat, Action OnHit)
    {
        _attackStat = attackStat;
        this.OnHit = OnHit;
    }

    public void OnTargetEnter(Collider2D collider)
    {
        ITarget target = collider.GetComponent<ITarget>();
        if (target == null)
        {
            OnHit?.Invoke();
            return;
        }

        if (target.IsTarget(_attackStat.DamageableData._targetType) == true)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage.Hit(_attackStat.DamageableData, damageable);
                OnHit?.Invoke();
                return;
            }
        }
    }
}
