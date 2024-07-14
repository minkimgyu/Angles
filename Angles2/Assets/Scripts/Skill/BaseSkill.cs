using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CastingData
{
    public CastingData(GameObject myObject, Transform myTransform)
    {
        _myObject = myObject;
        _myTransform = myTransform;
    }

    GameObject _myObject;
    public GameObject MyObject { get { return _myObject; } }

    Transform _myTransform;
    public Transform MyTransform { get { return _myTransform; } }
}

abstract public class BaseSkill
{
    public enum Name
    {
        Statikk,
        Knockback,
        Impact,

        SpawnBlackhole, // weapon
        SpawnShooter, // weapon
        SpawnBlade, // projectile
        SpawnStickyBomb, // projectile

        SpreadBullets,
        Shockwave,
        MagneticField,
        SelfDestruction
    }

    public virtual void Initialize(CastingData data) { }

    public abstract bool CanUse();

    public virtual void OnReflect(Collision2D collision) { }

    public virtual void OnCaptureEnter(ITarget target) { }
    public virtual void OnCaptureExit(ITarget target) { }

    public virtual void OnCaptureEnter(ITarget target, IDamageable damageable) { }
    public virtual void OnCaptureExit(ITarget target, IDamageable damageable) { }

    public virtual void OnUpdate() { }
    public virtual void OnAdd() { }
}
