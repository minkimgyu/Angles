using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Bullet : ProjectileWeapon
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

    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    void SpawnHitEffect()
    {
        BaseEffect effect = SpawnEffect?.Invoke(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void Initialize(BulletData data, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect)
    {
        this.SpawnEffect = SpawnEffect;

        _damage = data._damage;
        _lifeTime = data._lifeTime;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
