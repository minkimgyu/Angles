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

        AttachStickyBomb, // weapon

        SpawnBlackhole, // weapon
        SpawnShooter, // weapon
        SpawnSpear, // projectile
        SpawnBlade, // projectile
    }

    protected CastingData _castingData;
    protected float _probability;

    public void Initialize(CastingData data)
    {
        _castingData = data;
    }

    public abstract bool CanUse();

    public virtual void OnReflect(Collision2D collision) { }
    public virtual void OnTrigger(Collider2D collider) { }
    public virtual void OnUpdate() { }
    public virtual void OnAdd() { }
}
