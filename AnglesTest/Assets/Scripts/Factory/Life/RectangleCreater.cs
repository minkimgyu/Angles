using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Skill;

[System.Serializable]
public class RectangleData : EnemyData
{
    [JsonProperty] private float _moveSpeed;

    public RectangleData(UpgradeableStat<float> maxHp, ITarget.Type targetType, BaseEffect.Name dieEffectName, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd, 
        float moveSpeed) : base(maxHp, targetType, dieEffectName, size, skillDataToAdd)
    {
        _moveSpeed = moveSpeed;
        _skillData = skillDataToAdd;
    }

    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    public override LifeData Copy()
    {
        return new RectangleData(
            _maxHp.Copy(), // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _destroyEffectName,
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillData), // 딕셔너리 깊은 복사
            _moveSpeed // TriangleData 고유 값
        );
    }
}

public class RectangleCreater : LifeCreater
{
    BaseFactory _skillFactory;
    DropData _dropData;

    public RectangleCreater(BaseLife lifePrefab, LifeData lifeData, DropData dropData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
        _dropData = dropData;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        RectangleData data = CopyLifeData as RectangleData;

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