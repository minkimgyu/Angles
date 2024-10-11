using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class HexagonData : EnemyData
{
    public float _moveSpeed;
    public float _stopDistance;
    public float _gap;

    public HexagonData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData, float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, size, skillDataToAdd, dropData)
    {
        _moveSpeed = moveSpeed;
        CopySkillDataToAdd = skillDataToAdd;

        _stopDistance = stopDistance;
        _gap = gap;
    }

    public override LifeData Copy()
    {
        return new HexagonData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(CopySkillDataToAdd), // 딕셔너리 깊은 복사
            _dropData, // EnemyData에서 상속된 값
            _moveSpeed, // TriangleData 고유 값
            _stopDistance,
            _gap
        );
    }
}

public class HexagonCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public HexagonCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        HexagonData data = CopyLifeData as HexagonData;

        life.ResetData(data);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

        ISkillAddable skillUsable = life.GetComponent<ISkillAddable>();
        if (skillUsable == null) return life;

        foreach (var item in data.CopySkillDataToAdd)
        {
            BaseSkill skill = _skillFactory.Create(item.Key);
            skill.Upgrade(item.Value);
            skillUsable.AddSkill(item.Key, skill);
        }

        return life;
    }
}
