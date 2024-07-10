using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICondition
{
    void OnAdd();
    void OnReflect(Collision2D collision);
    void OnTrigger(Collider2D collider);
    void OnUpdate();
}

public struct CastingData
{
    public CastingData(Transform transform)
    {
        _myTransform = transform;
    }

    Transform _myTransform;
    public Transform MyTransform { get { return _myTransform; } }
}

abstract public class BaseSkill : ICondition
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
    }

    public virtual void Initialize(CastingData data) { }

    public abstract bool CanUse();

    public virtual void OnReflect(Collision2D collision) { }
    public virtual void OnTrigger(Collider2D collider) { }
    public virtual void OnUpdate() { }
    public virtual void OnAdd() { }
}
