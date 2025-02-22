using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableMissile : BaseWeapon, ITrackable
{
    BaseFactory _effectFactory;
    TrackableMissileData _data;

    public override void ModifyData(TrackableMissileDataModifier modifier)
    {
        modifier.Visit(_data);
    }

    public override void InjectData(TrackableMissileData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        base.Initialize(effectFactory);
        _effectFactory = effectFactory;
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

    public override void InitializeStrategy()
    {
        MoveComponent moveComponent = GetComponent<MoveComponent>();
        moveComponent.Initialize();

        TrackComponent trackComponent = GetComponent<TrackComponent>();
        trackComponent.Initialize(transform, BaseLife.Size.Small);

        _targetStrategy = new NoTargetingStrategy();
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new BulletAttackStrategy(_data, OnHit);
        _moveStrategy = new TrackingMoveStrategy(_data.MoveSpeed, moveComponent, trackComponent);
    }

    public void InjectTarget(ITarget target)
    {
        _moveStrategy.InjectTarget(target);
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath)
    {
    }
}
