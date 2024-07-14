using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public enum Name
    {
        Blade,
        Bullet,
        Rocket,

        Blackhole,
        Shooter,
        StickyBomb,
    }

    protected float _damage;
    protected List<ITarget.Type> _targetTypes;

    public virtual void Initialize(ShooterData data) { }
    public virtual void Initialize(RocketData data) { }
    public virtual void Initialize(BulletData data) { }
    public virtual void Initialize(BladeData data) { }
    public virtual void Initialize(BlackholeData data) { }
    public virtual void Initialize(StickyBombData data) { }

    public virtual void ResetFollower(IFollowable followable) { }

    public virtual void ResetPosition(Vector3 pos) { transform.position = pos; }
    public virtual void ResetPosition(Vector3 pos, Vector3 direction) { transform.position = pos; transform.right = direction; }
    public virtual void ResetPosition(IAttachable attachable) { }

    public void ResetDamage(float damage) { _damage = damage; }
    public void ResetTargetTypes(List<ITarget.Type> types) { _targetTypes = types; }
}
