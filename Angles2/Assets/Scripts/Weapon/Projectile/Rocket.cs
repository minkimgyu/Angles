using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : ProjectileWeapon
{
    List<RocketUpgradableData> _upgradableDatas;
    RocketUpgradableData UpgradableData { get { return _upgradableDatas[_upgradePoint - 1]; } }

    //float _explosionDamage;
    //float _explosionRange;

    System.Func<BaseEffect.Name, BaseEffect> SpawnEffect;

    public override void ResetData(RocketData data)
    {
        _upgradableDatas = data._upgradableDatas;
        _lifeTime = data._lifeTime;
    }

    public override void Initialize(System.Func<BaseEffect.Name, BaseEffect> SpawnEffect)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        this.SpawnEffect = SpawnEffect;
    }

    protected void ApplyDamage(IDamageable damageable)
    {
        DamageData damageData = new DamageData(UpgradableData.Damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 폭팔을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();
        DamageData damageData = new DamageData(UpgradableData.ExplosionDamage, _targetTypes);
        Damage.HitCircleRange(damageData, transform.position, UpgradableData.ExplosionRange, true, Color.red, 3);

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