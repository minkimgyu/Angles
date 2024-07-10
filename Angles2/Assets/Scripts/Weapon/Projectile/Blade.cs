using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Blade : Projectile
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
    float _attackDelay;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 nomal = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(transform.right, nomal);
        Shoot(direction);
    }

    public override void Initialize(BladeData data)
    {
        _damage = data._damage;
        _lifeTime = data._lifeTime;
        _force = data._force;
        _attackDelay = data._attackDelay;

        _targetDatas = new List<TargetData>();
        _captureComponent = GetComponentInChildren<DamageableCaptureComponent>();
        _captureComponent.Initialize(OnEnter, OnExit);
        _lifeTimer = new Timer();
        _lifeTimer.Start(_lifeTime);

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }

    void OnEnter(IDamageable damageable)
    {
        _targetDatas.Add(new TargetData(Time.time, damageable));
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

        for (int i = 0; i < _targetDatas.Count; i++)
        {
            float duration = Time.time - _targetDatas[i].CaptureTime;
            if (duration > _attackDelay)
            {
                DamageData damageData = new DamageData(_damage, _targetTypes);
                _targetDatas[i].Damageable.GetDamage(damageData);
                _targetDatas[i].CaptureTime = Time.time;
            }
        }
    }
}
