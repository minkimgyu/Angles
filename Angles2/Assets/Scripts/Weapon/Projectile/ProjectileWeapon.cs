using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ProjectileWeapon : BaseWeapon, IProjectable
{
    protected float _force;
    protected MoveComponent _moveComponent;

    public void Shoot(Vector3 direction, float force)
    {
        transform.right = direction;
        _force = force;

        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _force);
    }
}
