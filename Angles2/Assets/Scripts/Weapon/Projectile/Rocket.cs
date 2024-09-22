using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : ProjectileWeapon
{
    BaseFactory _effectFactory;
    RocketData _data;

    public override void ResetData(RocketData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _lifetimeComponent = new LifetimeComponent(_data);

        _effectFactory = effectFactory;
    }

    protected void ApplyDamage(IDamageable damageable)
    {
        DamageableData damageData = 
            
        new DamageableData.DamageableDataBuilder().
       SetDamage(new DamageData(_data._damage, _data._totalDamageRatio))
       .SetTargets(_data._targetTypes)
       .Build();

        damageable.GetDamage(damageData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 폭발을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();

        DamageableData damageData =

        new DamageableData.DamageableDataBuilder().
        SetDamage(new DamageData(_data._damage, _data._totalDamageRatio))
        .SetTargets(_data._targetTypes)
        .Build();


        Damage.HitCircleRange(damageData, transform.position, _data._range, true, Color.red, 3);
        Destroy(gameObject);
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
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
}