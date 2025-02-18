using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Bullet : BaseWeapon, IProjectable
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
       _attackStrategy.OnTargetEnter(collider);
    }

    BaseFactory _effectFactory;
    BulletData _data;

    void OnHit()
    {
        SpawnHitEffect();
        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void ModifyData(BulletDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void InjectData(BulletData data)
    {
        _data = data;
    }

    public void Shoot(Vector3 direction, float force)
    {
        transform.right = direction;
        _force = force;

        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _force);
    }

    protected float _force;
    protected MoveComponent _moveComponent;

    public override void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;
        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, OnHit);
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new BulletAttackStrategy(_data, OnHit);

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
