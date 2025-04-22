using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAttackStrategy : IWeaponActionStrategy
{
    float _attackDelay;
    DamageableData _damageableData;
    Func<List<BladeDetectingStrategy.TargetData>> GetDamageableTargets;

    public GearAttackStrategy(
        float attackDelay,
        DamageableData damageableData,
        Func<List<BladeDetectingStrategy.TargetData>> GetDamageableTargets)
    {
        _attackDelay = attackDelay;
        _damageableData = damageableData;
        this.GetDamageableTargets = GetDamageableTargets;
    }

    public void OnUpdate()
    {
        List<BladeDetectingStrategy.TargetData> targetDatas = GetDamageableTargets();

        for (int i = targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - targetDatas[i].CaptureTime;
            if (targetDatas[i].HitCount == 0 || duration > _attackDelay)
            {
                targetDatas[i].HitCount++;
                targetDatas[i].Damageable.GetDamage(_damageableData);

                if (i < 0 || targetDatas.Count - 1 < i) continue;
                targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
