using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    const float _groggyDuration = 1f;
    const float _damage = 10f;
    const float _force = 1.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if (target == null) return;

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        IForce forceTaret = collision.gameObject.GetComponent<IForce>();
        if (forceTaret == null) return;

        DamageableData damageData = new DamageableData
        (
            new DamageStat(_damage),
            _groggyDuration
        );

        Vector3 direction = forceTaret.GetPosition() - (Vector3)collision.contacts[0].point;
        forceTaret.ApplyForce(direction, _force, ForceMode2D.Impulse);
        damageable.GetDamage(damageData);
    }
}
