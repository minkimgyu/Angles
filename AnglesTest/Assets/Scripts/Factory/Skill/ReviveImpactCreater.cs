using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

[System.Serializable]
public class ReviveImpactData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _range;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public ReviveImpactData(
        int maxUpgradePoint,
        float range,
        float damage,
        List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new ReviveImpactData(
            _maxUpgradePoint, // RandomSkillData에서 상속된 값
            _range,
            _damage,
            new List<ITarget.Type>(_targetTypes) // 리스트의 깊은 복사
        );
    }
}

public class ReviveImpactCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ReviveImpactCreater(SkillData data, BaseFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        ReviveImpactData data = CopySkillData as ReviveImpactData;
        return new ReviveImpact(data, _effectFactory);
    }
}
