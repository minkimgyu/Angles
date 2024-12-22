using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
    [JsonProperty] private float _moveSpeed;
    [JsonProperty] private float _drainRatio;
    [JsonProperty] private float _drainPercentage;

    [JsonProperty] private float _chargeDuration;
    [JsonProperty] private float _dashSpeed;
    [JsonProperty] private float _dashDuration;

    [JsonProperty] private float _shootDuration;
    [JsonProperty] private float _shootSpeed;

    [JsonProperty] private float _minJoystickLength;
    [JsonProperty] private int _maxDashCount;
    [JsonProperty] private int _dashConsumeCount;

    [JsonProperty] private float _dashRestoreDuration;

    [JsonProperty] private float _shrinkScale;
    [JsonProperty] private float _normalScale;

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<BaseSkill.Name> _skillNames;

    public float AttackDamage { get; set; }
    public float TotalDamageRatio { get; set; }
    public float TotalCooltimeRatio { get; set; }
    public float TotalRandomRatio { get; set; }


    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    [JsonIgnore] public float DrainRatio { get => _drainRatio; set => _drainRatio = value; }
    [JsonIgnore] public float DrainPercentage { get => _drainPercentage; set => _drainPercentage = value; }
    [JsonIgnore] public float ChargeDuration { get => _chargeDuration; set => _chargeDuration = value; }
    [JsonIgnore] public float DashSpeed { get => _dashSpeed; set => _dashSpeed = value; }
    [JsonIgnore] public float DashDuration { get => _dashDuration; set => _dashDuration = value; }
    [JsonIgnore] public float ShootDuration { get => _shootDuration; set => _shootDuration = value; }
    [JsonIgnore] public float ShootSpeed { get => _shootSpeed; set => _shootSpeed = value; }
    [JsonIgnore] public float MinJoystickLength { get => _minJoystickLength; set => _minJoystickLength = value; }
    [JsonIgnore] public int MaxDashCount { get => _maxDashCount; set => _maxDashCount = value; }
    [JsonIgnore] public int DashConsumeCount { get => _dashConsumeCount; set => _dashConsumeCount = value; }
    [JsonIgnore] public float DashRestoreDuration { get => _dashRestoreDuration; set => _dashRestoreDuration = value; }
    [JsonIgnore] public float ShrinkScale { get => _shrinkScale; set => _shrinkScale = value; }
    [JsonIgnore] public float NormalScale { get => _normalScale; set => _normalScale = value; }
    [JsonIgnore] public List<BaseSkill.Name> SkillNames { get => _skillNames; set => _skillNames = value; }

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
            _drainPercentage,
            _moveSpeed,

            ChargeDuration,
            _dashSpeed,
            _dashDuration,
            _shootDuration,
            _shootSpeed,
            MinJoystickLength,
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
        float drainPercentage,

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
        _drainPercentage = drainPercentage;

        ChargeDuration = chargeDuration;

        _dashSpeed = dashSpeed;

        _dashDuration = dashDuration;

        _shootDuration = shootDuration;

        _shootSpeed = shootSpeed;

        MinJoystickLength = minJoystickLength;
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

        for (int i = 0; i < data.SkillNames.Count; i++)
        {
            BaseSkill skill = _skillFactory.Create(data.SkillNames[i]);
            skillUser.AddSkill(data.SkillNames[i], skill);
        }

        return life;
    }
}
