using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoUpgradeableData : IUpgradeableSkillData
{
    public float AttackDamage { get; set; } = 0;

    public float TotalDamageRatio { get; set; } = 1;
    public float TotalCooltimeRatio { get; set; } = 1;
    public float TotalRandomRatio { get; set; } = 1;
}

public interface IUpgradeableSkillData
{
    public float AttackDamage { get; set; }
    public float TotalDamageRatio { get; set; }
    public float TotalCooltimeRatio { get; set; }
    public float TotalRandomRatio { get; set; }
}

[System.Serializable]
public class PlayerData : LifeData, IUpgradeableSkillData
{
    public float _moveSpeed;
    public float _drainRatio;

    public float _chargeDuration;
    public float _dashSpeed;
    public float _dashDuration;

    public float _shootDuration;
    public float _shootSpeed;

    public float _minJoystickLength;
    public int _maxDashCount;
    public int _dashConsumeCount;

    public float _dashRestoreDuration;

    public float _shrinkScale;
    public float _normalScale;

    public List<BaseSkill.Name> _skillNames;

    public float AttackDamage { get; set; }

    public float TotalDamageRatio { get; set; }
    public float TotalCooltimeRatio { get; set; }
    public float TotalRandomRatio { get; set; }

    public override LifeData Copy()
    {
        return new PlayerData(
            _maxHp, // LifeData에서 상속된 값
            _targetType, // LifeData에서 상속된 값
            _autoHpRecoveryPoint, // LifeData에서 상속된 값
            _damageReductionRatio, // LifeData에서 상속된 값

            AttackDamage,
            TotalDamageRatio,
            TotalCooltimeRatio,
            TotalRandomRatio,

            _drainRatio,
            _moveSpeed,

            _chargeDuration,
            _dashSpeed,
            _dashDuration,
            _shootDuration,
            _shootSpeed,
            _minJoystickLength,
            _maxDashCount,
            _dashConsumeCount,
            _dashRestoreDuration,
            _shrinkScale,
            _normalScale,
            new List<BaseSkill.Name>(_skillNames) // 리스트의 경우 참조를 복사하는 대신 새 리스트로 복사
        );
    }

    public PlayerData(
        float maxHp, 
        ITarget.Type targetType,
        float autoRecoveryPoint, 
        float damageReductionRatio,

        float attackDamage,
        float totalDamageRatio,
        float totalCooltimeRatio,
        float totalRandomRatio,

        float drainRatio,
        float moveSpeed,

        float chargeDuration,

        float dashSpeed,

        float dashDuration,

        float shootDuration,

        float shootSpeed,
        float minJoystickLength, 
        int maxDashCount, 
        int dashConsumeCount, 

        float dashRestoreDuration,

        float shrinkScale, 
        float normalScale, 
        List<BaseSkill.Name> skillNames) : base(maxHp, targetType, autoRecoveryPoint, damageReductionRatio)
    {
        AttackDamage = attackDamage;
        TotalDamageRatio = totalDamageRatio;
        TotalCooltimeRatio = totalCooltimeRatio;
        TotalRandomRatio = totalRandomRatio;

        _moveSpeed = moveSpeed;
        _drainRatio = drainRatio;

        _chargeDuration = chargeDuration;

        _dashSpeed = dashSpeed;

        _dashDuration = dashDuration;

        _shootDuration = shootDuration;

        _shootSpeed = shootSpeed;

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

        ICaster skillUser = life.GetComponent<ICaster>();
        if (skillUser == null) return life;

        for (int i = 0; i < data._skillNames.Count; i++)
        {
            BaseSkill skill = _skillFactory.Create(data._skillNames[i]);
            skillUser.AddSkill(data._skillNames[i], skill);
        }

        return life;
    }
}
