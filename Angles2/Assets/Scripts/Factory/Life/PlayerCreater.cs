using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData : BaseLifeData
{
    public float _minTotalDamageRatio;
    public float _maxTotalDamageRatio;
    public float _totalDamageRatio;

    public float _minTotalCooltimeRatio;
    public float _maxTotalCooltimeRatio;
    public float _totalCooltimeRatio;

    public float _moveSpeed;

    public float _minChargeDuration;
    public float _maxChargeDuration;
    public float _chargeDuration;

    public float _minDashSpeed;
    public float _maxDashSpeed;
    public float _dashSpeed;

    public float _minDashDuration;
    public float _maxDashDuration;
    public float _dashDuration;

    public float _minShootDuration;
    public float _maxShootDuration;
    public float _shootDuration;

    public float _shootSpeed;

    public float _minJoystickLength;

    public int _maxDashCount;

    public int _dashConsumeCount;

    public float _minDashRestoreDuration;
    public float _maxDashRestoreDuration;
    public float _dashRestoreDuration;

    public float _shrinkScale;
    public float _normalScale;

    public List<BaseSkill.Name> _skillNames;

    public PlayerData(
        float maxHp, 
        ITarget.Type targetType,
        float moveSpeed,

        float minChargeDuration,
        float maxChargeDuration,
        float chargeDuration,

        float minTotalDamageRatio,
        float maxTotalDamageRatio,
        float totalDamageRatio,

        float minTotalCooltimeRatio,
        float maxTotalCooltimeRatio,
        float totalCooltimeRatio,

        float minDashSpeed,
        float maxDashSpeed,
        float dashSpeed,

        float minDashDuration,
        float maxDashDuration,
        float dashDuration,

        float minShootDuration,
        float maxShootDuration,
        float shootDuration,

        float shootSpeed,
        float minJoystickLength, 
        int maxDashCount, 
        int dashConsumeCount, 

        float minDashRestoreDuration,
        float maxDashRestoreDuration,
        float dashRestoreDuration,

        float shrinkScale, 
        float normalScale, 
        List<BaseSkill.Name> skillNames) : base(maxHp, targetType)
    {
        _moveSpeed = moveSpeed;

        _minChargeDuration = minChargeDuration;
        _maxChargeDuration = maxChargeDuration;
        _chargeDuration = chargeDuration;

        _minTotalDamageRatio = minTotalDamageRatio;
        _maxTotalDamageRatio = maxTotalDamageRatio;
        _totalDamageRatio = totalDamageRatio;

        _minTotalCooltimeRatio = minTotalCooltimeRatio;
        _maxTotalCooltimeRatio = maxTotalCooltimeRatio;
        _totalCooltimeRatio = totalCooltimeRatio;

        _minDashSpeed = minDashSpeed;
        _maxDashSpeed = maxDashSpeed;
        _dashSpeed = dashSpeed;

        _minDashDuration = minDashDuration;
        _maxDashDuration = maxDashDuration;
        _dashDuration = dashDuration;

        _minShootDuration = minShootDuration;
        _maxShootDuration = maxShootDuration;
        _shootDuration = shootDuration;

        _shootSpeed = shootSpeed;

        _minJoystickLength = minJoystickLength;
        _maxDashCount = maxDashCount;

        _dashConsumeCount = dashConsumeCount;

        _minDashRestoreDuration = minDashRestoreDuration;
        _maxDashRestoreDuration = maxDashRestoreDuration;
        _dashRestoreDuration = dashRestoreDuration;

        _shrinkScale = shrinkScale;
        _normalScale = normalScale;

        _skillNames = skillNames;
    }
}

public class PlayerCreater : LifeCreater
{
    BaseFactory _skillFactory;

    public PlayerCreater(BaseLife lifePrefab, BaseLifeData lifeData, 
        BaseFactory effectFactory, BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        PlayerData data = _lifeData as PlayerData;

        life.ResetData(data);
        life.Initialize();
        life.AddEffectFactory(_effectFactory);

        ISkillAddable skillUser = life.GetComponent<ISkillAddable>();
        if (skillUser == null) return life;

        for (int i = 0; i < data._skillNames.Count; i++)
        {
            BaseSkill skill = _skillFactory.Create(data._skillNames[i]);
            skillUser.AddSkill(data._skillNames[i], skill);
        }

        return life;
    }
}
