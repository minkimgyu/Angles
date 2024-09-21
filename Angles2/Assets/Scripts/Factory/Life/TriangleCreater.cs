using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TriangleData : EnemyData
{
    public float _moveSpeed;

    public TriangleData(float maxHp, ITarget.Type targetType, BaseLife.Size size, Dictionary<BaseSkill.Name, int> skillDataToAdd,
        DropData dropData, float moveSpeed) : base(maxHp, targetType, size, skillDataToAdd, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillDataToAdd = skillDataToAdd;
    }

    public override LifeData Copy()
    {
        return new TriangleData(
            _maxHp, // EnemyData에서 상속된 값
            _targetType, // EnemyData에서 상속된 값
            _size, // EnemyData에서 상속된 값
            new Dictionary<BaseSkill.Name, int>(_skillDataToAdd), // 딕셔너리 깊은 복사
            _dropData, // EnemyData에서 상속된 값
            _moveSpeed // TriangleData 고유 값
        );
    }
}

public class TriangleCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public TriangleCreater(BaseLife lifePrefab, LifeData lifeData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;
        TriangleData data = _lifeData as TriangleData;

        // level 변수 선언
        // 여기에 Upgrader를 넣어서 데이터를 업데이트 시켜주고
        // 아래에서 초기화해주면 어떨까?

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
