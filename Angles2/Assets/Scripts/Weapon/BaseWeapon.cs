using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public enum Name
    {
        Blade,
        Bullet,
        Spear,

        Blackhole,
        Shooter,
    }

    protected float _damage;
    protected List<ITarget.Type> _targetTypes;

    public virtual void Initialize(float damage, float moveSpeed, float fireMaxDelay, float offsetToFollower) { }
    public virtual void Initialize(float damage, float lifeTime, float reflectSpeed) { }
    public virtual void Initialize(float damage, float lifeTime, float absorbForce, int maxTargetCount) { }

    public virtual void ResetFollower(Transform follower) { }
    public virtual void ResetPosition(Vector3 pos) { }
    public void ResetDamageableTypes(List<ITarget.Type> types) { _targetTypes = types; }
}
