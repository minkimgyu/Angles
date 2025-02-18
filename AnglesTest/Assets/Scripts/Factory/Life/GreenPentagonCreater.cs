using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GreenPentagonData : EnemyData
{
    [JsonProperty] private float _moveSpeed;
    [JsonProperty] private float _stopDuration;
    [JsonProperty] private float _rushDuration;

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float StopDuration { get => _stopDuration; set => _stopDuration = value; }
    [JsonIgnore] public float RushDuration { get => _rushDuration; set => _rushDuration = value; }

    public GreenPentagonData(
        float maxHp,
        ITarget.Type targetType,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed,
        float stopDuration,
        float rushDuration) : base(maxHp, targetType, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _stopDuration = stopDuration;
        _rushDuration = rushDuration;
    }

    public override LifeData Copy()
    {
        return new GreenPentagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사
            _moveSpeed, // TriangleData 고유 값
            _stopDuration,
            _rushDuration
        );
    }
}

public class GreenPentagonCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public GreenPentagonCreater(BaseLife lifePrefab, LifeData lifeData, DropData dropData, BaseFactory SpawnEffect,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, SpawnEffect)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        GreenPentagonData data = CopyLifeData as GreenPentagonData;

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

