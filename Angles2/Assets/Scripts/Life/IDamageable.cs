using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public DamageData(float damage, List<ITarget.Type> damageableTypes)
    {
        _damage = damage;
        _targetType = damageableTypes;
    }

    float _damage;
    public float Damage { get { return _damage; } }

    List<ITarget.Type> _targetType;
    public List<ITarget.Type> DamageableTypes { get { return _targetType; } }
}

public interface ITarget : IPos
{
    public enum Type
    {
        Blue, // 플레이어 편
        Red, // 적 편
    }

    Type ReturnTargetType();
}

public interface IDamageable
{
    void GetDamage(DamageData damageData);
}
