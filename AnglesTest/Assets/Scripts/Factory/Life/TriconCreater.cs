using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriconData : EnemyData
{
    [JsonProperty] private float _moveSpeed;
    [JsonProperty] private float _stopDistance;
    [JsonProperty] private float _gap;

    [JsonProperty] private float _freezeDuration;
    [JsonProperty] private float _movableDuration;

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
    [JsonIgnore] public float Gap { get => _gap; set => _gap = value; }
    [JsonIgnore] public float FreezeDuration { get => _freezeDuration; set => _freezeDuration = value; }
    [JsonIgnore] public float MovableDuration { get => _movableDuration; set => _movableDuration = value; }

    public TriconData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed, float stopDistance, float gap, float freezeDuration, float movableDuration) : base(maxHp, targetType, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;

        _freezeDuration = freezeDuration;
        _movableDuration = movableDuration;
    }

    public override LifeData Copy()
    {
        return new TriconData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사
            _moveSpeed, // TriangleData 고유 값
            _stopDistance,
            _gap,
            _freezeDuration,
            _movableDuration
        );
    }
}

public class TriconCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public TriconCreater(BaseLife lifePrefab, LifeData lifeData, DropData dropData, BaseFactory SpawnEffect,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, SpawnEffect)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        TriconData data = CopyLifeData as TriconData;

        life.ResetData(data, _dropData);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ICaster skillAddable = life.GetComponent<ICaster>();
        if (skillAddable == null) return life;

        foreach (var item in data.SkillData)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            skillAddable.AddSkill(item.Key, skill);
        }

        return life;
    }
}
