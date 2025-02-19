using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackholeAttackStrategy : IAttackStrategy
{
    BlackholeData _data;
    Transform _myTransform;
    Func<List<ForceTargetingStrategy.TargetData>> GetTargets;

    public BlackholeAttackStrategy(
        BlackholeData data,
        Transform myTransform,
        Func<List<ForceTargetingStrategy.TargetData>> GetTargets)
    {
        _data = data;
        _myTransform = myTransform;
        this.GetTargets = GetTargets;
    }

    public void OnUpdate()
    {
        List<ForceTargetingStrategy.TargetData> targetDatas = GetTargets();

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
                Damage.Hit(_data.DamageableData, targetDatas[i].DamageableTarget);
                targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
