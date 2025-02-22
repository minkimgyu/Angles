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

    public HexagonData(float maxHp, ITarget.Type targetType, BaseEffect.Name dieEffectName, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, dieEffectName, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _skillData = skillDataToAdd;

        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new HexagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _destroyEffectName,
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사
            _moveSpeed, // TriangleData 고유 값
            _stopDistance,
            _gap
        );
    }
}

[System.Serializable]
public class OperaHexagonData : HexagonData
{
    [JsonProperty] private float _freezeDuration;
    [JsonProperty] private float _movableDuration;
    [JsonProperty] private float _freezeSpeed;

    [JsonIgnore] public float FreezeDuration { get => _freezeDuration; set => _freezeDuration = value; }
    [JsonIgnore] public float MovableDuration { get => _movableDuration; set => _movableDuration = value; }
    [JsonIgnore] public float FreezeSpeed { get => _freezeSpeed; set => _freezeSpeed = value; }

    public OperaHexagonData(
        float maxHp,
        ITarget.Type targetType,
        BaseEffect.Name dieEffectName,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed,
        float stopDistance,

        float freezeSpeed,
        float freezeDuration,
        float movableDuration,

        float gap) : base(maxHp, targetType, dieEffectName, size, skillDataToAdd, moveSpeed, stopDistance, gap)
    {
        _freezeDuration = freezeDuration;
        _movableDuration = movableDuration;
        _freezeSpeed = freezeSpeed;
    }

    public override LifeData Copy()
    {
        return new OperaHexagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _destroyEffectName,
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사

            _moveSpeed, // TriangleData 고유 값
            _stopDistance,

            _freezeSpeed,
            _freezeDuration,
            _movableDuration,

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

        life.InjectData(data, _dropData);
        life.InjectEffectFactory(_effectFactory);

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

        life.InjectData(data, _dropData);
        life.InjectEffectFactory(_effectFactory);

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