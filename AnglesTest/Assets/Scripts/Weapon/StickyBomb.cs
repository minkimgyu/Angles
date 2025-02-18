using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using Unity.VisualScripting;

public class StickyBomb : BaseWeapon
{
    IFollowable _followable;

    BaseFactory _effectFactory;
    StickyBombData _data;

    public override void InjectData(StickyBombData data)
    {
        _data = data;
    }

    public override void ModifyData(StickyBombDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void Initialize(BaseFactory effectFactory) 
    {
        _effectFactory = effectFactory;
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, OnLifetimeCompleted);
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new StickyBombAttackStrategy(transform, _data);
    }

    void Explode()
    {
        Debug.Log("Explode");
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();

        _attackStrategy.OnLifetimeCompleted();
    }

    public void OnLifetimeCompleted()
    {
        Explode();
        Destroy(gameObject);
    }

    public override void ResetFollower(IFollowable followable) 
    {
        _followable = followable;
    }

    protected override void Update()
    {
        base.Update();
        if (_followable as UnityEngine.Object == null) return;

        bool canAttach = _followable.CanFollow();
        if (canAttach == true)
        {
            Vector3 pos = _followable.GetPosition();
            transform.position = pos;
        }
    }
}
