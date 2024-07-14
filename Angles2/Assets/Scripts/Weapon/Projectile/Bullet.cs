using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Bullet : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IObstacle obstacle = collision.GetComponent<IObstacle>();
        if (obstacle != null)
        {
            Destroy(gameObject);
            SpawnHitEffect();
            return;
        }

        ITarget target = collision.GetComponent<ITarget>();
        if (target == null) return;
        if (target.IsTarget(_targetTypes) == false) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) ApplyDamage(damageable);

        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Hit);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void Initialize(BulletData data)
    {
        _damage = data._damage;
        _lifeTime = data._lifeTime;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
