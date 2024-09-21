using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DamageUtility;

public struct DamageableData
{
    public DamageData _damageData;
    public List<ITarget.Type> _targetType;
    public float _groggyDuration;

    public class DamageableDataBuilder
    {
        private DamageData _damage = new DamageData();
        private List<ITarget.Type> _targetType = new List<ITarget.Type> { ITarget.Type.Blue, ITarget.Type.Red };
        private float _groggyDuration = 0f;

        // Damage 설정
        public DamageableDataBuilder SetDamage(DamageData damage)
        {
            _damage = damage;
            return this;
        }

        // Damageable 대상 설정
        public DamageableDataBuilder SetTargets(List<ITarget.Type> targetTypes)
        {
            _targetType = targetTypes;
            return this;
        }

        // Groggy 시간 설정
        public DamageableDataBuilder SetGroggyDuration(float groggyDuration)
        {
            _groggyDuration = groggyDuration;
            return this;
        }

        // DamageableData 생성
        public DamageableData Build()
        {
            DamageableData data;
            data._damageData = _damage;
            data._targetType = _targetType;
            data._groggyDuration = _groggyDuration;

            return data;
        }
    }
}

public struct DamageData
{
    public float Damage { get { return _originDamage * _totalMultiplier; } }

    float _originDamage;
    float _totalMultiplier;

    public DamageData(float originDamage, float totalMultiplier) 
    { 
        _originDamage = originDamage;
        _totalMultiplier = totalMultiplier;
    }
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
    void GetDamage(DamageableData damageData);
}
