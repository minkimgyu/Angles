using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Bullet : ProjectileWeapon
{
    List<BulletUpgradableData> _upgradableDatas;
    BulletUpgradableData UpgradableData { get { return _upgradableDatas[_upgradePoint - 1]; } }

    protected void ApplyDamage(IDamageable damageable)
    {
        DamageData damageData = new DamageData(UpgradableData.Damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

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

    BaseFactory _effectFactory;

    void SpawnHitEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.HitEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void ResetData(BulletData data)
    {
        _upgradableDatas = data._upgradableDatas;
        _lifeTime = data._lifeTime;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        _effectFactory = effectFactory;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }
}
