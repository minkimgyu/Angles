using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Bullet : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ApplyDamage(collision);

        // 여기에 깨지는 파티클 넣어주기
        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void Initialize(BulletData data)
    {
        _damage = data._damage;
        _lifeTime = data._lifeTime;
        _force = data._force;

        _moveComponent = GetComponent<MoveComponent>();
    }
}
