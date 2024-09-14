using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TriangleData : EnemyData
{
    public float _moveSpeed;

    public TriangleData(float maxHp, ITarget.Type targetType, BaseEnemy.Size size, List<BaseSkill.Name> skillNames,
        DropData dropData, float moveSpeed) : base(maxHp, targetType, size, skillNames, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;
    }
}

public class TriangleCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public TriangleCreater(BaseLife lifePrefab, BaseLifeData lifeData, BaseFactory effectFactory,
        BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        TriangleData data = _lifeData as TriangleData;

        life.ResetData(data);
        life.Initialize();
        life.AddEffectFactory(_effectFactory);

        ISkillAddable skillUsable = life.GetComponent<ISkillAddable>();
        if (skillUsable == null) return life;

        for (int i = 0; i < data._skillNames.Count; i++)
        {
            BaseSkill skill = _skillFactory.Create(data._skillNames[i]);
            skillUsable.AddSkill(data._skillNames[i], skill);
        }

        return life;
    }
}
