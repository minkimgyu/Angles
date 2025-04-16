using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using Skill.Strategy;

public class StickyBomb : BaseWeapon
{
    BaseFactory _effectFactory;
    StickyBombData _data;

    public override void InjectData(StickyBombData data)
    {
        _data = data;
    }

    public override void ModifyData(StickyBombDataModifier modifier)
    {
        modifier.Visit(_data);
        _targetingStrategy.InjectTargetTypes(_data.TargetTypes);
        _lifeTimeStrategy.ChangeLifetime(_data.Lifetime);
    }

    public override void Initialize(BaseFactory effectFactory) 
    {
        base.Initialize(effectFactory);
        _effectFactory = effectFactory;
    }

    void Explode()
    {
        Debug.Log("Explode");
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    void OnHit(List<IDamageable> damageables)
    {
        _actionStrategy.Execute(damageables, _data.DamageableStat);
        Explode();
        Destroy(gameObject);
    }

    public override void InitializeStrategy()
    {
        base.InitializeStrategy();
        FollowComponent followComponent = GetComponent<FollowComponent>();
        followComponent.Initialize();

        _targetingStrategy = new CircleRangeTargetingStrategy(_data.Range, transform, OnHit);
        _actionStrategy = new HitTargetStrategy();
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy
        (
            () => 
            { 
                _targetingStrategy.Execute(); 
            }
        );
        _moveStrategy = new FollowingMoveStrategy(followComponent);
    }
}
