using DamageUtility;
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
    BlackholeData _data;

    public override void ResetPosition(Vector3 pos)
    {
        base.ResetPosition(pos);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Blackhole, pos);
    }

    public override void ResetData(BlackholeData data)
    {
        _data = data;
    }

    public override void ModifyData(List<WeaponDataModifier> modifiers)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            _data = modifiers[i].Visit(_data);
        }
    }

    public override void Initialize() 
    {
        _targetDatas = new List<TargetData>();
        _lifetimeComponent = new LifetimeComponent(_data, () => { Destroy(gameObject); });
        _sizeModifyComponent = new SizeModifyComponent(transform, _data);

        _absorbCaptureComponent = GetComponentInChildren<AbsorbableCaptureComponent>();
        _absorbCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(IForce absorbable, IDamageable damageable)
    {
        //if (_targetDatas.Count >= _data._maxTargetCount) return;
        _targetDatas.Add(new TargetData(Time.time, absorbable, damageable));
    }

    void OnExit(IForce absorbable, IDamageable damageable)
    {
        TargetData data = _targetDatas.Find(x => x.AbsorbableTarget == absorbable && x.DamageableTarget == damageable);
        _targetDatas.Remove(data);
    }

    protected override void Update()
    {
        base.Update();
        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if(duration > _data._forceDelay)
            {
                Vector3 direction = _targetDatas[i].AbsorbableTarget.ReturnPosition() - transform.position;
                _targetDatas[i].AbsorbableTarget.ApplyForce(direction, _data._absorbForce, ForceMode2D.Force);
                Damage.Hit(_data._damageableData, _targetDatas[i].DamageableTarget);

                if (i < 0 || _targetDatas.Count - 1 < i) continue;
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
