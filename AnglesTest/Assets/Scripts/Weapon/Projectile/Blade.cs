using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Blade : BaseWeapon, IProjectable
{
    DamageableCaptureComponent _captureComponent;
    BladeData _data;
    Timer _soundTimer;

    float _force;
    MoveComponent _moveComponent;

    public void Shoot(Vector3 direction, float force)
    {
        transform.right = direction;
        _force = force;

        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _force);
        _soundTimer.Start(1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);

        Vector2 nomal = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(transform.right, nomal);
        Shoot(direction, _force);
    }

    public override void ModifyData(BladeDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void InjectData(BladeData data)
    {
        _data = data;
    }

    public override void Initialize()
    {
        _soundTimer = new Timer();
        _captureComponent = GetComponentInChildren<DamageableCaptureComponent>();
        _captureComponent.Initialize(OnEnter, OnExit);

        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new ChangeableSizeStrategy(transform, _data);
        _attackStrategy = new BladeAttackStrategy(_data);

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }

    void OnEnter(IDamageable damageable)
    {
        _attackStrategy.OnTargetEnter(damageable);
    }

    void OnExit(IDamageable damageable)
    {
        _attackStrategy.OnTargetExit(damageable);
    }

    protected override void Update()
    {
        base.Update();

        if (_soundTimer.CurrentState == Timer.State.Finish)
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Blade, transform.position);
            _soundTimer.Reset();
            _soundTimer.Start(_data.AttackDelay);
        }

        _attackStrategy.OnUpdate();
    }
}
