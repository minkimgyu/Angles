using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

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

    public override void InitializeStrategy()
    {
        FollowComponent followComponent = GetComponent<FollowComponent>();
        followComponent.Initialize();

        _targetStrategy = new NoTargetingStrategy();
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new StickyBombAttackStrategy(transform, _data);
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy
        (
            _data,
            () =>
            {
                _attackStrategy.OnLifetimeCompleted();
                Explode();
                Destroy(gameObject);
            }
        );
        _moveStrategy = new FollowingMoveStrategy(followComponent);
    }
}
