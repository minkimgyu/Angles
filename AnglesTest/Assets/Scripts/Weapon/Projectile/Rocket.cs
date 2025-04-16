using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Rocket : BaseWeapon, IProjectable
{
    BaseFactory _effectFactory;
    RocketData _data;

    public override void InjectData(RocketData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        base.Initialize(effectFactory);

        _effectFactory = effectFactory;
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    void OnHit(List<IDamageable> damageables)
    {
        // ������ �켱 �����ϰ� ���Ŀ� ������ ���� ���� �������� ���Ѵ�.
        SpawnExplosionEffect();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);
        _actionStrategy.Execute(damageables, _data.DamageableStat);
        Destroy(gameObject);
    }

    public override void ModifyData(RocketDataModifier modifier)
    {
        modifier.Visit(_data);

        _targetingStrategy.InjectTargetTypes(_data.TargetTypes);
        _lifeTimeStrategy.ChangeLifetime(_data.Lifetime);
    }

    public override void InitializeStrategy()
    {
        base.InitializeStrategy();
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        _targetingStrategy = new ContactRangeTargetingStrategy(
            _data.Range,
            transform,
            OnHit
        );

        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(() => { Destroy(gameObject); });
        _actionStrategy = new HitTargetStrategy();
        _moveStrategy = new ProjectileMoveStrategy(moveComponent, transform);
    }

    public void Shoot(Vector3 direction, float force)
    {
        _moveStrategy.Shoot(direction, force);
    }
}