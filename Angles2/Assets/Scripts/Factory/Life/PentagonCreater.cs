using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PentagonData : EnemyData
{
    public float _moveSpeed;
    public float _stopDistance;
    public float _gap;

    public PentagonData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData, float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, size, skillDataToAdd, dropData)
    {
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new PentagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillDataToAdd), // 딕셔너리 깊은 복사
            _dropData, // EnemyData에서 상속된 값
            _moveSpeed, // TriangleData 고유 값
            _stopDistance,
            _gap
        );
    }
}

public class PentagonCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public PentagonCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory SpawnEffect,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, SpawnEffect)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        PentagonData data = _lifeData as PentagonData;

        life.ResetData(data);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ISkillAddable skillUsable = life.GetComponent<ISkillAddable>();
        if (skillUsable == null) return life;

        foreach (var item in data._skillDataToAdd)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            skillUsable.AddSkill(item.Key, skill);
        }

        return life;
    }
}
