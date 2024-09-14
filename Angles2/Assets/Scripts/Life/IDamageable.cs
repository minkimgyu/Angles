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
        _showTxt = true;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = 0;
        _damageTxtColor = Color.white;
        _showTxt = true;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes, float groggyDuration)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = groggyDuration;
        _damageTxtColor = Color.white;
        _showTxt = true;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes, float groggyDuration, bool showTxt)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = groggyDuration;
        _damageTxtColor = Color.white;
        _showTxt = showTxt;
    }

    public DamageData(float damage, List<ITarget.Type> damageableTypes, float groggyDuration, bool showTxt, Color damageTxtColor)
    {
        _damage = damage;
        _targetType = damageableTypes;
        _groggyDuration = groggyDuration;
        _showTxt = showTxt;
        _damageTxtColor = damageTxtColor;
    }

    float _damage;
    public float Damage { get { return _damage; } }

    bool _showTxt;
    public bool ShowTxt { get { return _showTxt; } }

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

public class DefaultTarget : ITarget
{
    ITarget.Type _type;

    public DefaultTarget(ITarget.Type types)
    {
        _type = types;
    }

    public bool IsTarget(List<ITarget.Type> types)
    {
        return types.Contains(_type);
    }

    public Vector3 ReturnPosition()
    {
        return Vector3.zero;
    }
}

public interface IDamageable
{
    void GetDamage(DamageData damageData);
}
