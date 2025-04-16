using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

[System.Serializable]
public class MagneticFieldData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _delay;

    [JsonProperty] private float _adRatio;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }

    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public MagneticFieldData(int maxUpgradePoint, float damage, float adRatio, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _delay = delay;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new MagneticFieldData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _damage,
            _adRatio,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}

public class MagneticFieldCreater : SkillCreater
{
    IUpgradeVisitor _upgrader;

    public MagneticFieldCreater(SkillData data, IUpgradeVisitor upgrader) : base(data)
    {
        _upgrader = upgrader;
    }

    public override BaseSkill Create()
    {
        MagneticFieldData data = CopySkillData.Copy() as MagneticFieldData;
        return new MagneticField(data, _upgrader);
    }
}