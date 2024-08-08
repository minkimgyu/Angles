using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : ProjectileWeapon
{
    float _explosionDamage;
    float _explosionRange;

    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public override void Initialize(RocketData data, System.Func<BaseEffect.Name, BaseEffect> SpawnEffect)
    {
        _damage = data._damage;
        _lifeTime = data._lifeTime;
        _force = data._force;

        _explosionDamage = data._explosionDamage;
        _explosionRange = data._explosionRange;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        this.SpawnEffect = SpawnEffect;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 폭팔을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();
        DamageData damageData = new DamageData(_explosionDamage, _targetTypes);
        Damage.HitCircleRange(damageData, transform.position, _explosionRange, true, Color.red, 3);

        ITarget target = collision.GetComponent<ITarget>();
        if (target != null && target.IsTarget(_targetTypes) == true)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null) ApplyDamage(damageable);
        }

        Destroy(gameObject);
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = SpawnEffect?.Invoke(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }
}