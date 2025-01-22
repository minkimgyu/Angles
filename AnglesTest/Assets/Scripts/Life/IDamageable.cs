using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

[Serializable]
public struct DamageableData
{
    public ICaster _caster; // ������ ������ ���
    public DamageStat _damageStat;
    public List<ITarget.Type> _targetType;
    public float _groggyDuration;

    public float CalculateDamage(float damageReductionRatio = 0)
    {
        float totalDamage = _damageStat.TotalDamage;
        float reducedDamage = _damageStat.TotalDamage * damageReductionRatio;
        return totalDamage - reducedDamage;
    }

    public DamageableData(DamageStat damageStat)
    {
        _caster = null;
        _damageStat = damageStat;
        _targetType = new List<ITarget.Type> { ITarget.Type.Red, ITarget.Type.Blue, ITarget.Type.Construction };
        _groggyDuration = 0;
    }

    public DamageableData(DamageStat damageStat, float groggyDuration)
    {
        _caster = null;
        _damageStat = damageStat;
        _targetType = new List<ITarget.Type> { ITarget.Type.Red, ITarget.Type.Blue, ITarget.Type.Construction };
        _groggyDuration = groggyDuration;
    }

    public DamageableData(ICaster caster, DamageStat damageStat)
    {
        _caster = caster;
        _damageStat = damageStat;
        _targetType = new List<ITarget.Type> { ITarget.Type.Red, ITarget.Type.Blue, ITarget.Type.Construction };
        _groggyDuration = 0;
    }

    public DamageableData(ICaster caster, DamageStat damageStat, List<ITarget.Type> targetType, float groggyDuration = 0)
    {
        _caster = caster;
        _damageStat = damageStat;
        _targetType = targetType;
        _groggyDuration = groggyDuration;
    }
}

public struct DamageStat
{
    public float TotalDamage 
    { 
        get 
        { 
            return (_originDamage + (_attackDamage * _adRatio)) * _totalMultiplier;  // (�������� ������ + (AD * AD �ݿ� ����)) * ������ ���� ����
        } 
    }

    float _originDamage;
    float _totalMultiplier;

    float _attackDamage; // ad ��ġ
    float _adRatio; // ad �� �ݿ� ����

    public DamageStat(float originDamage)
    {
        _originDamage = originDamage;
        _attackDamage = 0;
        _adRatio = 0;
        _totalMultiplier = 1;
    }

    public DamageStat(float originDamage, float attackDamage, float adRatio, float totalMultiplier) 
    { 
        _originDamage = originDamage;
        _attackDamage = attackDamage;
        _adRatio = adRatio;
        _totalMultiplier = totalMultiplier;
    }
}

public interface IPos
{
    Vector3 GetPosition();
}

public interface ITarget : IPos
{
    public enum Type
    {
        Blue, // �÷��̾� ��
        Red, // �� ��
        Construction // �ǹ�
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

    public Vector3 GetPosition()
    {
        return Vector3.zero;
    }
}

public interface IDamageable
{
    void GetDamage(DamageableData damageData);
}
