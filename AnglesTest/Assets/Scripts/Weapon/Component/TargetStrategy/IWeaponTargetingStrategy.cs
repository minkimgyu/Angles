using DrawDebug;
using Skill.Strategy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IWeaponTargetingStrategy
{
    void InjectTargetTypes(List<ITarget.Type> targetTypes) { }
    void Execute() { }
    void OnTriggerEnter(Collider2D collision) { }
}

public class NoTargetingStrategy : IWeaponTargetingStrategy
{
}

public class ContactTargetingStrategy : IWeaponTargetingStrategy
{
    List<ITarget.Type> _targetTypes;
    Action<IDamageable> OnHit;

    public ContactTargetingStrategy(
        Action<IDamageable> OnHit)
    {
        this.OnHit = OnHit;
    }

    public void InjectTargetTypes(List<ITarget.Type> targetTypes) 
    {
        _targetTypes = targetTypes;
    }

    public void OnTriggerEnter(Collider2D collider) 
    {
        ITarget target = collider.GetComponent<ITarget>();
        if (target == null) // 벽의 경우
        {
            OnHit?.Invoke(null);
            return;
        }

        if (target.IsTarget(_targetTypes) == true)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable == null) OnHit?.Invoke(null);

            OnHit?.Invoke(damageable);
            return;
        }
    }
}

public class ContactRangeTargetingStrategy : IWeaponTargetingStrategy
{
    float _range;
    Transform _myTransform;

    List<ITarget.Type> _targetTypes;
    Action<List<IDamageable>> OnHit;

    public ContactRangeTargetingStrategy(
        float range,
        Transform myTransform,
        Action<List<IDamageable>> OnHit)
    {
        _range = range;
        _myTransform = myTransform;
        this.OnHit = OnHit;
    }

    public void InjectTargetTypes(List<ITarget.Type> targetTypes)
    {
        _targetTypes = targetTypes;
    }

    public void OnTriggerEnter(Collider2D collider)
    {
        ITarget target = collider.GetComponent<ITarget>();
        if (target != null && target.IsTarget(_targetTypes) == false) return; // 타겟이 아닌 경우

        // 벽이거나 타겟인 경우

        List<IDamageable> damageables = new List<IDamageable>();

#if UNITY_EDITOR
        DebugShape.DrawCircle2D(_myTransform.position, _range, Color.red, 3f);
#endif
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_myTransform.position, _range);

        for (int i = 0; i < colliders.Length; i++)
        {
            ITarget closeTarget = colliders[i].GetComponent<ITarget>();
            if (closeTarget == null || closeTarget.IsTarget(_targetTypes) == false) continue;

            IDamageable damageable = colliders[i].GetComponent<IDamageable>();
            if (damageable == null) continue;

            damageables.Add(damageable);
        }
        
        OnHit?.Invoke(damageables);
        return;
    }
}

public class CircleRangeTargetingStrategy : IWeaponTargetingStrategy
{
    float _range;
    Transform _myTransform;

    List<ITarget.Type> _targetTypes;
    Action<List<IDamageable>> OnHit;

    public CircleRangeTargetingStrategy(
        float range,
        Transform myTransform,
        Action<List<IDamageable>> OnHit)
    {
        _range = range;
        _myTransform = myTransform;
        this.OnHit = OnHit;
    }

    public void InjectTargetTypes(List<ITarget.Type> targetTypes)
    {
        _targetTypes = targetTypes;
    }

    public void Execute()
    {
        // 벽이거나 타겟인 경우
        List<IDamageable> damageables = new List<IDamageable>();

#if UNITY_EDITOR
        DebugShape.DrawCircle2D(_myTransform.position, _range, Color.red, 3f);
#endif
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_myTransform.position, _range);

        for (int i = 0; i < colliders.Length; i++)
        {
            ITarget closeTarget = colliders[i].GetComponent<ITarget>();
            if (closeTarget == null || closeTarget.IsTarget(_targetTypes) == false) continue;

            IDamageable damageable = colliders[i].GetComponent<IDamageable>();
            if (damageable == null) continue;

            damageables.Add(damageable);
        }

        OnHit?.Invoke(damageables);
        return;
    }
}