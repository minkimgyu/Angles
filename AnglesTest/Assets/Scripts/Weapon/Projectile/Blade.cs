using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Blade : BaseWeapon, IProjectable
{
    BladeData _data;
    Timer _soundTimer;

    public override void ModifyData(BladeDataModifier modifier)
    {
        modifier.Visit(_data);
    }

    public override void InjectData(BladeData data)
    {
        _data = data;
    }

    public override void Initialize()
    {
        base.Initialize();
        _soundTimer = new Timer();
        _soundTimer.Start(1f);
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
    }

    public override void InitializeStrategy()
    {
        DamageableCaptureComponent damageableCaptureComponent = GetComponentInChildren<DamageableCaptureComponent>();
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        _targetStrategy = new DamageableTargetingStrategy(damageableCaptureComponent, _data);
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new ChangeableSizeStrategy(transform, _data);
        _attackStrategy = new BladeAttackStrategy(_data, _targetStrategy.GetDamageableTargets);
        _moveStrategy = new ReflectableProjectileMoveStrategy(moveComponent, transform);
    }

    public void Shoot(Vector3 direction, float force)
    {
        _moveStrategy.Shoot(direction, force);
    }
}
