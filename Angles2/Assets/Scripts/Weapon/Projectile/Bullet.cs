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
        if (target.IsTarget(_data._targetTypes) == false) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            DamageableData damageData =

           new DamageableData.DamageableDataBuilder().
           SetDamage(new DamageData(_data._damage, _data._totalDamageRatio))
           .SetTargets(_data._targetTypes)
           .Build();

            damageable.GetDamage(damageData);
        }

        Destroy(gameObject);
    }

    BaseFactory _effectFactory;
    BulletData _data;
    Timer _lifeTimer;

    void SpawnHitEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void ModifyData(List<WeaponDataModifier> modifiers)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            _data = modifiers[i].Visit(_data);
        }
    }

    public override void ResetData(BulletData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;
        _lifetimeComponent = new LifetimeComponent(_data);

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
