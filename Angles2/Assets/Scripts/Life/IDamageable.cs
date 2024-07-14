using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DamageData
{
    public DamageData(float damage)
    {
        _damage = damage;

        // 모든 타겟을 다 넣어준다.
        _targetType = new List<ITarget.Type>();
        foreach (ITarget.Type type in Enum.GetValues(typeof(ITarget.Type)))
        {
            _targetType.Add(type);
        }

        _groggyDuration = 0;
        _damageTxtColor = Color.white;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = 0;
        _damageTxtColor = Color.white;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes, float groggyDuration)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = groggyDuration;
        _damageTxtColor = Color.white;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes, float groggyDuration, Color damageTxtColor)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = groggyDuration;
        _damageTxtColor = damageTxtColor;
    }

    float _damage;
    public float Damage { get { return _damage; } }

    List<ITarget.Type> _targetType;
    public List<ITarget.Type> DamageableTypes { get { return _targetType; } }

    float _groggyDuration;
    public float GroggyDuration { get { return _groggyDuration; } }

    Color _damageTxtColor;
    public Color DamageTxtColor { get { return _damageTxtColor; } }
}

public interface ITarget : IPos
{
    public enum Type
    {
        Blue, // 플레이어 편
        Red, // 적 편
    }

    bool IsTarget(List<Type> types);
}

public interface IDamageable
{
    void GetDamage(DamageData damageData);
}
