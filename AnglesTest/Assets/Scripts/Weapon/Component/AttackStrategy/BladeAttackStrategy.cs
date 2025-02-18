using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAttackStrategy : IAttackStrategy
{
    public class TargetData
    {
        public TargetData(float captureTime, IDamageable damageable)
        {
            _captureTime = captureTime;
            _damageable = damageable;
        }

        public float CaptureTime { get { return _captureTime; } set { _captureTime = value; } }
        public IDamageable Damageable { get { return _damageable; } }

        float _captureTime;
        IDamageable _damageable;
    }

    BladeData _bladeData;

    public BladeAttackStrategy(BladeData bladeData)
    {
        _targetDatas = new List<TargetData>();
        _bladeData = bladeData;
    }

    List<TargetData> _targetDatas;

    public void OnTargetEnter(IDamageable damageable) 
    {
        _targetDatas.Add(new TargetData(Time.time, damageable));
        Damage.Hit(_bladeData.DamageableData, damageable);
    }

    public void OnTargetExit(IDamageable damageable) 
    {
        TargetData targetData = _targetDatas.Find(x => x.Damageable == damageable);
        _targetDatas.Remove(targetData);
    }

    public void OnUpdate()
    {
        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if (duration > _bladeData.AttackDelay)
            {
                Damage.Hit(_bladeData.DamageableData, _targetDatas[i].Damageable);

                if (i < 0 || _targetDatas.Count - 1 < i) continue;
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
