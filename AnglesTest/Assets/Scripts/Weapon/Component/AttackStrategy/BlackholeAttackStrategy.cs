using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeAttackStrategy : IAttackStrategy
{
    public class TargetData
    {
        public TargetData(float captureTime, IForce absorbableTarget, IDamageable damageableTarget)
        {
            _captureTime = captureTime;
            _absorbableTarget = absorbableTarget;
            _damageableTarget = damageableTarget;
        }

        public float CaptureTime { get { return _captureTime; } set { _captureTime = value; } }
        public IForce AbsorbableTarget { get { return _absorbableTarget; } }
        public IDamageable DamageableTarget { get { return _damageableTarget; } }

        float _captureTime;
        IForce _absorbableTarget;
        IDamageable _damageableTarget;
    }

    List<TargetData> _targetDatas;
    BlackholeData _data;
    Transform _myTransform;

    public BlackholeAttackStrategy(BlackholeData data, Transform myTransform)
    {
        _targetDatas = new List<TargetData>();
        _data = data;
        _myTransform = myTransform;
    }

    public void OnTargetEnter(IForce absorbable, IDamageable damageable, ITarget target) 
    {
        if (target.IsTarget(_data.DamageableData._targetType) == false) return;
        _targetDatas.Add(new TargetData(Time.time, absorbable, damageable));
    }

    public void OnTargetExit(IForce absorbable, IDamageable damageable, ITarget target)
    {
        TargetData data = _targetDatas.Find(x => x.AbsorbableTarget == absorbable && x.DamageableTarget == damageable);
        _targetDatas.Remove(data);
    }

    public void OnUpdate()
    {
        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if (duration > _data.AbsorbForce)
            {
                if (_targetDatas[i].AbsorbableTarget as UnityEngine.Object == null)
                {
                    _targetDatas.RemoveAt(i);
                    continue;
                }

                Vector3 direction = _targetDatas[i].AbsorbableTarget.GetPosition() - _myTransform.position;
                _targetDatas[i].AbsorbableTarget.ApplyForce(direction, _data.AbsorbForce, ForceMode2D.Force);
                Damage.Hit(_data.DamageableData, _targetDatas[i].DamageableTarget);
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
