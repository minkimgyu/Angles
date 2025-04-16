using DamageUtility;
using DrawDebug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponDetectingStrategy
{
    void InjectTargetTypes(List<ITarget.Type> targetTypes) { }

    List<ITarget> GetTargets() { return default; }
    List<BladeDetectingStrategy.TargetData> GetBladeTargets() { return default; }
    List<BlackholeDetectingStrategy.TargetData> GetForceTargets() { return default; }
}

public class NoDetectingStrategy : IWeaponDetectingStrategy
{
}

public class TargetDetectingStrategy : IWeaponDetectingStrategy
{
    TargetCaptureComponent _targetCaptureComponent;
    List<ITarget> _targetDatas;

    List<ITarget.Type> _targetTypes;

    public TargetDetectingStrategy(TargetCaptureComponent targetCaptureComponent)
    {
        _targetCaptureComponent = targetCaptureComponent;
        _targetCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
        _targetDatas = new List<ITarget>();
    }

    public void InjectTargetTypes(List<ITarget.Type> targetTypes) { _targetTypes = targetTypes; }

    public List<ITarget> GetTargets() { return _targetDatas; }

    void OnTargetEnter(ITarget target)
    {
        if (target.IsTarget(_targetTypes) == false) return;
        _targetDatas.Add(target);
    }

    void OnTargetExit(ITarget target)
    {
        _targetDatas.Remove(target);
    }
}

public class BladeDetectingStrategy : IWeaponDetectingStrategy
{
    public class TargetData
    {
        public TargetData(float captureTime, ITarget target, IDamageable damageable)
        {
            _captureTime = captureTime;
            _target = target;
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
    List<ITarget.Type> _targetTypes;

    public void InjectTargetTypes(List<ITarget.Type> targetTypes) { _targetTypes = targetTypes; }

    public BladeDetectingStrategy(DamageableCaptureComponent damageableCaptureComponent)
    {
        _damageableCaptureComponent = damageableCaptureComponent;
        _damageableCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
        _targetDatas = new List<TargetData>();
    }

    public List<TargetData> GetBladeTargets() { return _targetDatas; }

    void OnTargetEnter(ITarget target, IDamageable damageable)
    {
        // 확인은 Attack 클래스에서 진행
        if (target.IsTarget(_targetTypes) == false) return;
        _targetDatas.Add(new TargetData(Time.time, target, damageable));
    }

    void OnTargetExit(ITarget target, IDamageable damageable)
    {
        TargetData targetData = _targetDatas.Find(x => x.Target == target);
        _targetDatas.Remove(targetData);
    }
}

public class BlackholeDetectingStrategy : IWeaponDetectingStrategy
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

    public void InjectTargetTypes(List<ITarget.Type> targetTypes) { _targetTypes = targetTypes; }


    AbsorbableCaptureComponent _absorbCaptureComponent;
    List<TargetData> _targetDatas;
    List<ITarget.Type> _targetTypes;

    public BlackholeDetectingStrategy(AbsorbableCaptureComponent absorbCaptureComponent)
    {
        _targetDatas = new List<TargetData>();
        _absorbCaptureComponent = absorbCaptureComponent;
        _absorbCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
    }

    public List<TargetData> GetForceTargets() { return _targetDatas; }

    void OnTargetEnter(ITarget target, IForce absorbable, IDamageable damageable)
    {
        // 확인은 Attack 클래스에서 진행
        if (target.IsTarget(_targetTypes) == false) return;
        _targetDatas.Add(new TargetData(Time.time, target, absorbable, damageable));
    }

    void OnTargetExit(ITarget target, IForce absorbable, IDamageable damageable)
    {
        TargetData data = _targetDatas.Find(x => x.Target == target);
        _targetDatas.Remove(data);
    }
}