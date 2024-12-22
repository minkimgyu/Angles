using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class ContactAttackData : SkillData
{
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _groggyDuration;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float GroggyDuration { get => _groggyDuration; set => _groggyDuration = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public ContactAttackData(int maxUpgradePoint, float damage, float adRatio, float groggyDuration, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _damage = damage;
        _adRatio = adRatio;
        _groggyDuration = groggyDuration;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
       return new ContactAttackData(
           _maxUpgradePoint,
           _damage,
           _adRatio,
           _groggyDuration,
           _targetTypes
       );
    }
}

public class ContactAttackCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public ContactAttackCreater(SkillData data, BaseFactory _effectFactory) : base(data)
    {
        this._effectFactory = _effectFactory;
    }

    public override BaseSkill Create()
    {
        ContactAttackData data = CopySkillData as ContactAttackData;
        return new ContactAttack(data, _effectFactory);
    }
}