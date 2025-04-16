using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

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
        UpgradeableStat<float> maxHp,
        ITarget.Type targetType,
        BaseEffect.Name dieEffectName,
        BaseLife.Size size,
        Dictionary<BaseSkill.Name, int> skillDataToAdd,
        float moveSpeed,
        float stopDuration,
        float rushDuration) : base(maxHp, targetType, dieEffectName, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _stopDuration = stopDuration;
        _rushDuration = rushDuration;
    }

    public override LifeData Copy()
    {
        return new GreenPentagonData(
            _maxHp.Copy(), // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _destroyEffectName,
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

        life.InjectData(data, _dropData);
        life.InjectEffectFactory(_effectFactory);

        life.Initialize();

        ICaster caster = life.GetComponent<ICaster>();
        if (caster == null) return life;

        foreach (var item in data.SkillData)
        {
            for (int i = 0; i <= item.Value; i++)
            {
                BaseSkill skill = _skillFactory.Create(item.Key);
                caster.AddSkill(item.Key, skill);
            }
        }

        return life;
    }
}

