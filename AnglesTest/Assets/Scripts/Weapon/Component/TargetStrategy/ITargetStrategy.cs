using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface ITargetStrategy
{
    List<ITarget> GetTargets() { return default; }
    List<DamageableTargetingStrategy.TargetData> GetDamageableTargets() { return default; }
    List<ForceTargetingStrategy.TargetData> GetForceTargets() { return default; }
}

public class NoTargetingStrategy : ITargetStrategy
{
}

public class TargetTargetingStrategy : ITargetStrategy
{
    TargetCaptureComponent _targetCaptureComponent;
    List<ITarget> _targetDatas;

    public TargetTargetingStrategy(TargetCaptureComponent targetCaptureComponent)
    {
        _targetCaptureComponent = targetCaptureComponent;
        _targetCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
        _targetDatas = new List<ITarget>();
    }

    public List<ITarget> GetTargets() { return _targetDatas; }

    void OnTargetEnter(ITarget target)
    {
        _targetDatas.Add(target);
    }

    void OnTargetExit(ITarget target)
    {
        _targetDatas.Remove(target);
    }
}

public class DamageableTargetingStrategy : ITargetStrategy
{
    public class TargetData
    {
        public TargetData(float captureTime, ITarget target, IDamageable damageable)
        {
            _captureTime = captureTime;
            _damageable = damageable;
            _hitCount = 0;
        }

        public int HitCount { get { return _hitCount; } set { _hitCount = value; } }
        public float CaptureTime { get { return _captureTime; } set { _captureTime = value; } }
        public ITarget Target { get { return _target; } }
        public IDamageable Damageable { get { return _damageable; } }

        int _hitCount;
        float _captureTime;
        ITarget _target;
        IDamageable _damageable;
    }

    List<TargetData> _targetDatas;
    DamageableCaptureComponent _damageableCaptureComponent;
    IAttackStat _attackStat;

    public DamageableTargetingStrategy(DamageableCaptureComponent damageableCaptureComponent, IAttackStat attackStat)
    {
        _attackStat = attackStat;
        _damageableCaptureComponent = damageableCaptureComponent;
        _damageableCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
        _targetDatas = new List<TargetData>();
    }

    public List<TargetData> GetDamageableTargets() { return _targetDatas; }

    void OnTargetEnter(ITarget target, IDamageable damageable)
    {
        // 확인은 Attack 클래스에서 진행
        if (target.IsTarget(_attackStat.DamageableData._targetType) == false) return;
        _targetDatas.Add(new TargetData(Time.time, target, damageable));
    }

    void OnTargetExit(ITarget target, IDamageable damageable)
    {
        TargetData targetData = _targetDatas.Find(x => x.Target == target && x.Damageable == damageable);
        _targetDatas.Remove(targetData);
    }
}

public class ForceTargetingStrategy : ITargetStrategy
{
    public class TargetData
    {
        public TargetData(float captureTime, ITarget target, IForce absorbableTarget, IDamageable damageableTarget)
        {
            _captureTime = captureTime;
            _target = target;
            _absorbableTarget = absorbableTarget;
            _damageableTarget = damageableTarget;
            _hitCount = 0;
        }

        public int HitCount { get { return _hitCount; } set { _hitCount = value; } }
        public float CaptureTime { get { return _captureTime; } set { _captureTime = value; } }

        public ITarget Target { get { return _target; } }
        public IForce AbsorbableTarget { get { return _absorbableTarget; } }
        public IDamageable DamageableTarget { get { return _damageableTarget; } }

        int _hitCount;
        float _captureTime;
        ITarget _target;
        IForce _absorbableTarget;
        IDamageable _damageableTarget; // 어차피 타입 달라서 데미지 안 들어감
    }

    AbsorbableCaptureComponent _absorbCaptureComponent;
    List<TargetData> _targetDatas;
    IAttackStat _attackStat;

    public ForceTargetingStrategy(AbsorbableCaptureComponent absorbCaptureComponent, IAttackStat attackStat)
    {
        _attackStat = attackStat;
        _targetDatas = new List<TargetData>();
        _absorbCaptureComponent = absorbCaptureComponent;
        _absorbCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
    }

    public List<TargetData> GetForceTargets() { return _targetDatas; }


    public void OnTargetEnter(ITarget target, IForce absorbable, IDamageable damageable)
    {
        // 확인은 Attack 클래스에서 진행
        if (target.IsTarget(_attackStat.DamageableData._targetType) == false) return;
        _targetDatas.Add(new TargetData(Time.time, target, absorbable, damageable));
    }

    public void OnTargetExit(ITarget target, IForce absorbable, IDamageable damageable)
    {
        TargetData data = _targetDatas.Find(x => x.Target == target && x.AbsorbableTarget == absorbable && x.DamageableTarget == damageable);
        _targetDatas.Remove(data);
    }
}