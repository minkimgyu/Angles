using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BladeAttackStrategy : IWeaponActionStrategy
{
    BladeData _bladeData;
    Func<List<BladeTargetingStrategy.TargetData>> GetDamageableTargets;

    public BladeAttackStrategy(
        BladeData bladeData,
        Func<List<BladeTargetingStrategy.TargetData>> GetDamageableTargets)
    {
        _bladeData = bladeData;
        this.GetDamageableTargets = GetDamageableTargets;
    }

    public void OnUpdate()
    {
        List<BladeTargetingStrategy.TargetData> targetDatas = GetDamageableTargets();

        for (int i = targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - targetDatas[i].CaptureTime;
            if (targetDatas[i].HitCount == 0 || duration > _bladeData.AttackDelay)
            {
                targetDatas[i].HitCount++;
                Damage.Hit(_bladeData.DamageableData, targetDatas[i].Damageable);

                if (i < 0 || targetDatas.Count - 1 < i) continue;
                targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
