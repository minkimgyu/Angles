using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BladeAttackStrategy : IWeaponActionStrategy
{
    BladeData _bladeData;
    Func<List<BladeDetectingStrategy.TargetData>> GetDamageableTargets;

    public BladeAttackStrategy(
        BladeData bladeData,
        Func<List<BladeDetectingStrategy.TargetData>> GetDamageableTargets)
    {
        _bladeData = bladeData;
        this.GetDamageableTargets = GetDamageableTargets;
    }

    public void OnUpdate()
    {
        List<BladeDetectingStrategy.TargetData> targetDatas = GetDamageableTargets();

        for (int i = targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - targetDatas[i].CaptureTime;
            if (targetDatas[i].HitCount == 0 || duration > _bladeData.AttackDelay)
            {
                targetDatas[i].HitCount++;
                targetDatas[i].Damageable.GetDamage(_bladeData.DamageableStat);

                if (i < 0 || targetDatas.Count - 1 < i) continue;
                targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
