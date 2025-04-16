using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Skill;

[Serializable]
public class KnockbackData : CooltimeSkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _rangeMultiplier;

    [JsonProperty] private float _adRatio;
    [JsonProperty] private SerializableVector2 _size;
    [JsonProperty] private SerializableVector2 _offset;

    [JsonProperty] private float _force;

    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;
    [JsonProperty] private float _groggyDuration;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float RangeMultiplier { get => _rangeMultiplier; set => _rangeMultiplier = value; }

    [JsonIgnore] public float AdRatio { get => _adRatio; }
    [JsonIgnore] public SerializableVector2 Size { get => _size; }
    [JsonIgnore] public SerializableVector2 Offset { get => _offset; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; }
    [JsonIgnore] public float Force { get => _force; }

    public KnockbackData(
        int maxUpgradePoint,
        float coolTime,
        int maxStackCount,
        float damage,
        float rangeMultiplier,

        float adRatio,
        float groggyDuration,
        SerializableVector2 size,
        SerializableVector2 offset,
        float force,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint, coolTime, maxStackCount)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _rangeMultiplier = rangeMultiplier;
        _size = size;
        _offset = offset;
        _force = force;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new KnockbackData(
            _maxUpgradePoint, // CooltimeSkillData에서 상속된 값
            _coolTime, // CooltimeSkillData에서 상속된 값
            _maxStackCount, // CooltimeSkillData에서 상속된 값
            _damage,
            _rangeMultiplier,
            _adRatio,
            _groggyDuration,
            new SerializableVector2(_size.x, _size.y), // SerializableVector2 깊은 복사
            new SerializableVector2(_offset.x, _offset.y), // SerializableVector2 깊은 복사
            _force,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class KnockbackCreater : SkillCreater
{
    BaseFactory _effectFactory;
    IUpgradeVisitor _upgrader;

    public KnockbackCreater(SkillData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(data)
    {
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        KnockbackData data = CopySkillData as KnockbackData;
        return new Knockback(data, _upgrader, _effectFactory);
    }
}
