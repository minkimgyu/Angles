using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : Projectile
{
    float _explosionDamage;
    float _explosionRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IObstacle obstacle = collision.GetComponent<IObstacle>();
        if (obstacle != null)
        {
            Destroy(gameObject);
            SpawnExplosionEffect();
            return;
        }

        ITarget target = collision.GetComponent<ITarget>();
        if (target == null) return;
        if (target.IsTarget(_targetTypes) == false) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) ApplyDamage(damageable);

        Destroy(gameObject);

        DamageData damageData = new DamageData(_explosionDamage, _targetTypes);
        Damage.HitCircleRange(damageData, transform.position, _explosionRange, true, Color.red, 3);

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetPosition(transform.position);
        effect.Play();

        Destroy(gameObject);
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void Initialize(RocketData data)
    {
        _damage = data._damage;
        _lifeTime = data._lifeTime;
        _force = data._force;

        _explosionDamage = data._explosionDamage;
        _explosionRange = data._explosionRange;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}