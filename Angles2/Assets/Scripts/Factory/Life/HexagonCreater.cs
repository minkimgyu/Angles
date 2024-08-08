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

    public HexagonData(float maxHp, ITarget.Type targetType, List<BaseSkill.Name> skillNames,
        DropData dropData, float moveSpeed, float stopDistance, float gap) : base(maxHp, targetType, skillNames, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;

        _stopDistance = stopDistance;
        _gap = gap;
    }
}

public class HexagonCreater : LifeCreater
{
    Func<BaseSkill.Name, BaseSkill> CreateSkill;

    public HexagonCreater(BaseLife lifePrefab, BaseLifeData lifeData, Func<BaseEffect.Name, BaseEffect> CreateEffect,
        Func<BaseSkill.Name, BaseSkill> CreateSkill) : base(lifePrefab, lifeData, CreateEffect)
    {
        this.CreateSkill = CreateSkill;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        HexagonData data = _lifeData as HexagonData;

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
