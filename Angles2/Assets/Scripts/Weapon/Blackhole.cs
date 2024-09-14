using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : BaseWeapon
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

    AbsorbableCaptureComponent _absorbCaptureComponent;
    List<TargetData> _targetDatas;
    Timer _lifeTimer;
    BlackholeData _data;

    public override void ResetData(BlackholeData data)
    {
        _data = data;
        ResetSize(_data._range);
    }

    public override void Initialize() 
    {
        _targetDatas = new List<TargetData>();
        _lifeTimer = new Timer();

        _absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _absorbCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(IForce absorbable, IDamageable damageable)
    {
        if (_targetDatas.Count >= _data._maxTargetCount) return;
        _targetDatas.Add(new TargetData(Time.time, absorbable, damageable));
    }

    void OnExit(IForce absorbable, IDamageable damageable)
    {
        TargetData data = _targetDatas.Find(x => x.AbsorbableTarget == absorbable && x.DamageableTarget == damageable);
        _targetDatas.Remove(data);
    }

    private void Update()
    {
        if(_lifeTimer.CurrentState == Timer.State.Finish)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if(duration > _data._forceDelay)
            {
                _targetDatas[i].AbsorbableTarget.ApplyForce(transform.position, _data._absorbForce, ForceMode2D.Force);

                DamageData damageData = new DamageData(0, _targetTypes, 1f, false);
                _targetDatas[i].DamageableTarget.GetDamage(damageData);

                if (i < 0 || _targetDatas.Count - 1 < i) continue;
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
