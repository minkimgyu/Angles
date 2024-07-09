using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : BaseWeapon, IProjectile
{
    protected float _reflectSpeed;
    protected MoveComponent _moveComponent;

    public Vector2 ReturnDirectionVector()
    {
        return transform.right;
    }

    protected void ApplyDamage(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        DamageData damageData = new DamageData(_damage, _targetTypes);
        damageable.GetDamage(damageData);
    }

    public void Shoot(Vector3 direction)
    {
        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _reflectSpeed);
    }
}
