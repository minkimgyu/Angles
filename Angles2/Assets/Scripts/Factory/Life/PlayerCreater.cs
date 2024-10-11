using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoUpgradeableRatio : IUpgradeableRatio
{
    public float TotalDamageRatio { get; set; } = 1;
    public float TotalCooltimeRatio { get; set; } = 1;
    public float TotalRandomRatio { get; set; } = 1;
}

public interface IUpgradeableRatio
{
    public float TotalDamageRatio { get; set; }
    public float TotalCooltimeRatio { get; set; }
    public float TotalRandomRatio { get; set; }
}

[System.Serializable]
public class PlayerData : LifeData, IUpgradeableRatio
{
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

    public float TotalDamageRatio { get; set; }
    public float TotalCooltimeRatio { get; set; }
    public float TotalRandomRatio { get; set; }

    public override LifeData Copy()
    {
        return new PlayerData(
            _maxHp, // LifeData에서 상속된 값
            _targetType, // LifeData에서 상속된 값
            _moveSpeed,
            _minChargeDuration,
            _maxChargeDuration,
            _chargeDuration,
            _minDashSpeed,
            _maxDashSpeed,
            _dashSpeed,
            _minDashDuration,
            _maxDashDuration,
            _dashDuration,
            _minShootDuration,
            _maxShootDuration,
            _shootDuration,
            _shootSpeed,
            _minJoystickLength,
            _maxDashCount,
            _dashConsumeCount,
            _minDashRestoreDuration,
            _maxDashRestoreDuration,
            _dashRestoreDuration,
            _shrinkScale,
            _normalScale,
            new List<BaseSkill.Name>(_skillNames) // 리스트의 경우 참조를 복사하는 대신 새 리스트로 복사
        );
    }

    public PlayerData(
        float maxHp, 
        ITarget.Type targetType,
        float moveSpeed,

        float minChargeDuration,
        float maxChargeDuration,
        float chargeDuration,

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
        TotalDamageRatio = 1;
        TotalCooltimeRatio = 1;
        TotalRandomRatio = 1;

        _moveSpeed = moveSpeed;

        _minChargeDuration = minChargeDuration;
        _maxChargeDuration = maxChargeDuration;
        _chargeDuration = chargeDuration;

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

    public PlayerCreater(BaseLife lifePrefab, LifeData lifeData, 
        BaseFactory effectFactory, BaseFactory skillFactory) : base(lifePrefab, lifeData, effectFactory)
    {
        _skillFactory = skillFactory;
    }

    public override BaseLife Create()
    {
        BaseLife life = UnityEngine.Object.Instantiate(_lifePrefab);
        if (life == null) return null;

        PlayerData data = CopyLifeData as PlayerData;

        life.ResetData(data);
        life.AddEffectFactory(_effectFactory);

        life.Initialize();

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
