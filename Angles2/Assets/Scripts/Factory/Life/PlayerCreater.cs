using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData : BaseLifeData
{
    public float _moveSpeed;
    public float _chargeDuration;
    public float _maxChargePower;

    public float _dashSpeed;
    public float _dashDuration;

    public float _shootSpeed;
    public float _shootDuration;

    public float _minJoystickLength;

    public int _maxDashCount;

    public int _dashConsumeCount;
    public float _dashRestoreDuration;

    public float _shrinkScale;
    public float _normalScale;

    public List<BaseSkill.Name> _skillNames;

    public PlayerData(float maxHp, ITarget.Type targetType,
        float moveSpeed, float chargeDuration, float maxChargePower, float dashSpeed, float dashDuration, 
        float shootSpeed, float shootDuration,
        float minJoystickLength, int maxDashCount, 
        int dashConsumeCount, float dashRestoreDuration,

        float shrinkScale, float normalScale, List<BaseSkill.Name> skillNames) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;
        _chargeDuration = chargeDuration;
        _maxChargePower = maxChargePower;

        _dashSpeed = dashSpeed;
        _dashDuration = dashDuration;

        _shootSpeed = shootSpeed;
        _shootDuration = shootDuration;

        _minJoystickLength = minJoystickLength;
        _maxDashCount = maxDashCount;

        _dashConsumeCount = dashConsumeCount;
        _dashRestoreDuration = dashRestoreDuration;

        _shrinkScale = shrinkScale;
        _normalScale = normalScale;

        _skillNames = skillNames;
    }
}

public class PlayerCreater : LifeCreater
{
    Func<BaseSkill.Name, BaseSkill> CreateSkill;

    public PlayerCreater(BaseLife lifePrefab, BaseLifeData lifeData, 
        Func<BaseEffect.Name, BaseEffect> CreateEffect, Func<BaseSkill.Name, BaseSkill> CreateSkill) : base(lifePrefab, lifeData, CreateEffect)
    {
        this.CreateSkill = CreateSkill;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        PlayerData data = _lifeData as PlayerData;

        life.ResetData(data);
        life.Initialize();
        life.AddCreateEvent(CreateEffect, CreateSkill);

        ISkillUser skillUser = life.GetComponent<ISkillUser>();
        if (skillUser == null) return life;

        for (int i = 0; i < data._skillNames.Count; i++)
        {
            skillUser.AddSkill(data._skillNames[i]);
        }

        return life;
    }
}
