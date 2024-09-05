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

    BaseFactory _effectFactory;

    public override void ResetData(RocketData data)
    {
        _upgradableDatas = data._upgradableDatas;
        _lifeTime = data._lifeTime;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _effectFactory = effectFactory;
    }

    protected void ApplyDamage(IDamageable damageable)
    {
        DamageData damageData = new DamageData(UpgradableData.Damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ �켱 �����ϰ� ���Ŀ� ������ ���� ���� �������� ���Ѵ�.
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
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }
}