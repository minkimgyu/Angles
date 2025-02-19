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

    void OnHit()
    {
        // 폭발을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);
        Damage.HitCircleRange(_data.DamageableData, transform.position, _data.Range, true, Color.red, 3);
        Destroy(gameObject);
    }

    public override void ModifyData(RocketDataModifier modifier)
    {
        modifier.Visit(_data);
    }

    public override void InitializeStrategy()
    {
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        _targetStrategy = new NoTargetingStrategy();
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new RocketAttackStrategy(_data, OnHit);
        _moveStrategy = new ProjectileMoveStrategy(moveComponent, transform);
    }

    public void Shoot(Vector3 direction, float force)
    {
        _moveStrategy.Shoot(direction, force);
    }
}