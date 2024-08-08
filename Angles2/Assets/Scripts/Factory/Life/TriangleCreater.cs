using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TriangleData : EnemyData
{
    public float _moveSpeed;

    public TriangleData(float maxHp, ITarget.Type targetType, List<BaseSkill.Name> skillNames,
        DropData dropData, float moveSpeed) : base(maxHp, targetType, skillNames, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;
    }
}

public class TriangleCreater : LifeCreater
{
    Func<BaseSkill.Name, BaseSkill> CreateSkill;

    public TriangleCreater(BaseLife lifePrefab, BaseLifeData lifeData, Func<BaseEffect.Name, BaseEffect> CreateEffect,
        Func<BaseSkill.Name, BaseSkill> CreateSkill) : base(lifePrefab, lifeData, CreateEffect)
    {
        this.CreateSkill = CreateSkill;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        TriangleData data = _lifeData as TriangleData;

        life.ResetData(data);
        life.Initialize();
        life.AddCreateEvent(CreateEffect);

        ISkillUser skillUsable = life.GetComponent<ISkillUser>();
        if (skillUsable == null) return life;

        for (int i = 0; i < data._skillNames.Count; i++)
        {
            BaseSkill skill = CreateSkill?.Invoke(data._skillNames[i]);
            skillUsable.AddSkill(data._skillNames[i], skill);
        }

        return life;
    }
}
