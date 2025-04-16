using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackholeAttackStrategy : IWeaponActionStrategy
{
    BlackholeData _data;
    Transform _myTransform;
    Func<List<BlackholeDetectingStrategy.TargetData>> GetTargets;

    public BlackholeAttackStrategy(
        BlackholeData data,
        Transform myTransform,
        Func<List<BlackholeDetectingStrategy.TargetData>> GetTargets)
    {
        _data = data;
        _myTransform = myTransform;
        this.GetTargets = GetTargets;
    }

    public void OnUpdate()
    {
        List<BlackholeDetectingStrategy.TargetData> targetDatas = GetTargets();

        for (int i = targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - targetDatas[i].CaptureTime;
            if (duration > _data.AbsorbForce)
            {
                if (targetDatas[i].AbsorbableTarget as UnityEngine.Object == null)
                {
                    targetDatas.RemoveAt(i);
                    continue;
                }

                targetDatas[i].HitCount++;

                Vector3 direction = targetDatas[i].AbsorbableTarget.GetPosition() - _myTransform.position;
                targetDatas[i].AbsorbableTarget.ApplyForce(direction, _data.AbsorbForce, ForceMode2D.Force);
                targetDatas[i].DamageableTarget.GetDamage(_data.DamageableStat);
                targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
