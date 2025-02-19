using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class Bullet : BaseWeapon, IProjectable
{
    BaseFactory _effectFactory;
    BulletData _data;

    public override void ModifyData(BulletDataModifier modifier)
    {
        modifier.Visit(_data);
    }

    public override void InjectData(BulletData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        base.Initialize(effectFactory);
        _effectFactory = effectFactory;
    }

    public void Shoot(Vector3 direction, float force)
    {
        _moveStrategy.Shoot(direction, force);
    }

    public override void InitializeStrategy()
    {
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize(); // 초기화 후 Inject

        _targetStrategy = new NoTargetingStrategy();
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, OnHit);
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new BulletAttackStrategy(_data, OnHit);
        _moveStrategy = new ProjectileMoveStrategy(moveComponent, transform);
    }

    void SpawnHitEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    void OnHit()
    {
        SpawnHitEffect();
        Destroy(gameObject);
    }
}
