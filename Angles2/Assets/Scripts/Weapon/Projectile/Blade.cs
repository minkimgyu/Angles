using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Blade : BaseProjectile, IAttachable
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        DamageData damageData = new DamageData(_damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

    public override void Initialize(float damage, float reflectSpeed) 
    {
        _moveComponent = GetComponent<MoveComponent>();
        _damage = damage;
        _reflectSpeed = reflectSpeed;
    }

    public bool CanAttach() { return true; }

    public Vector3 ReturnPosition() { return transform.position; }
}
