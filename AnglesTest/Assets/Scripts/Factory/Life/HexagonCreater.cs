using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class HexagonData : EnemyData
{
    [JsonProperty] protected float _moveSpeed;
    [JsonProperty] protected float _stopDistance;
    [JsonProperty] protected float _gap;

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
    [JsonIgnore] public float Gap { get => _gap; set => _gap = value; }

    public HexagonData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _skillData = skillDataToAdd;

        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new HexagonData(
            _maxHp, // EnemyData���� ��ӵ� ��
            _targetType, // EnemyData���� ��ӵ� ��
            _size, // EnemyData���� ��ӵ� ��
            new Dictionary<BaseSkill.Name, int>(_skillData), // ��ųʸ� ���� ����
            _moveSpeed, // TriangleData ���� ��
            _stopDistance,
            _gap
        );
    }
}

public class NormalHexagonCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public NormalHexagonCreater(
        BaseLife lifePrefab,
        LifeData lifeData,
        DropData dropData,
        BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        HexagonData data = CopyLifeData as HexagonData;

        life.ResetData(data, _dropData);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ICaster caster = life.GetComponent<ICaster>();
        if (caster == null) return life;

        foreach (var item in data.SkillData)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            caster.AddSkill(item.Key, skill);
        }

        return life;
    }
}


public class OperaHexagonCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public OperaHexagonCreater(
        BaseLife lifePrefab,
        LifeData lifeData,
        DropData dropData,
        BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;



        OperaHexagonData data = CopyLifeData as OperaHexagonData;

        life.ResetData(data, _dropData);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ICaster caster = life.GetComponent<ICaster>();
        if (caster == null) return life;

        foreach (var item in data.SkillData)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            caster.AddSkill(item.Key, skill);
        }

        return life;
    }
}