using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Blade : ProjectileWeapon
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

    DamageableCaptureComponent _captureComponent;
    List<TargetData> _targetDatas;
    Timer _lifeTimer;
    //float _attackDelay;

    List<BladeUpgradableData> _upgradableDatas;
    BladeUpgradableData CurrentUpgradableData { get { return _upgradableDatas[_upgradePoint - 1]; } }

    protected void ApplyDamage(IDamageable damageable)
    {
        DamageData damageData = new DamageData(CurrentUpgradableData.Damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);

        Vector2 nomal = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(transform.right, nomal);
        Shoot(direction, _force);
    }

    public override void ResetData(BladeData data)
    {
        _upgradableDatas = data._upgradableDatas;
        _lifeTime = data._lifeTime;
        _lifeTimer.Start(_lifeTime);
        ResetSize(CurrentUpgradableData.Range);
    }

    public override void Initialize()
    {
        _targetDatas = new List<TargetData>();
        _captureComponent = GetComponentInChildren<DamageableCaptureComponent>();
        _captureComponent.Initialize(OnEnter, OnExit);
        _lifeTimer = new Timer();

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }

    void OnEnter(IDamageable damageable)
    {
        _targetDatas.Add(new TargetData(Time.time, damageable));
        ApplyDamage(damageable);
    }

    void OnExit(IDamageable damageable)
    {
        TargetData targetData = _targetDatas.Find(x => x.Damageable == damageable);
        _targetDatas.Remove(targetData);
    }

    private void Update()
    {
        if (_lifeTimer.CurrentState == Timer.State.Finish)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if (duration > CurrentUpgradableData.AttackDelay)
            {
                ApplyDamage(_targetDatas[i].Damageable);

                if (i < 0 || _targetDatas.Count - 1 < i) continue;
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
