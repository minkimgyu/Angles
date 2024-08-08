using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RectangleData : EnemyData
{
    public float _moveSpeed;

    public RectangleData(float maxHp, ITarget.Type targetType, List<BaseSkill.Name> skillNames,
        DropData dropData, float moveSpeed) : base(maxHp, targetType, skillNames, dropData)
    {
        _moveSpeed = moveSpeed;
        _skillNames = skillNames;
    }
}

public class RectangleCreater : LifeCreater
{
    Func<BaseSkill.Name, BaseSkill> CreateSkill;

    public RectangleCreater(BaseLife lifePrefab, BaseLifeData lifeData, Func<BaseEffect.Name, BaseEffect> SpawnEffect,
        Func<BaseSkill.Name, BaseSkill> CreateSkill) : base(lifePrefab, lifeData, SpawnEffect)
    {
        this.CreateSkill = CreateSkill;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        RectangleData data = _lifeData as RectangleData;

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