using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultipleShockwaveData : SkillData
{
    [JsonProperty] private float _waveSizeMultiply;
    [JsonProperty] private float _waveDelay;
    [JsonProperty] private int _maxWaveCount;

    [JsonProperty] private float _delay;
    [JsonProperty] private float _damage;
    [JsonProperty] private float _adRatio;
    [JsonProperty] private float _range;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))] private List<ITarget.Type> _targetTypes;

    [JsonIgnore] public float WaveSizeMultiply { get => _waveSizeMultiply; set => _waveSizeMultiply = value; }
    [JsonIgnore] public float WaveDelay { get => _waveDelay; set => _waveDelay = value; }
    [JsonIgnore] public int MaxWaveCount { get => _maxWaveCount; set => _maxWaveCount = value; }
    [JsonIgnore] public float Delay { get => _delay; set => _delay = value; }
    [JsonIgnore] public float Damage { get => _damage; set => _damage = value; }
    [JsonIgnore] public float AdRatio { get => _adRatio; set => _adRatio = value; }
    [JsonIgnore] public float Range { get => _range; set => _range = value; }
    [JsonIgnore] public List<ITarget.Type> TargetTypes { get => _targetTypes; set => _targetTypes = value; }

    public MultipleShockwaveData(int maxUpgradePoint, float waveSizeMultiply, float waveDelay, int maxWaveCount, float damage, float adRatio, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _waveSizeMultiply = waveSizeMultiply;
        _waveDelay = waveDelay;
        _maxWaveCount = maxWaveCount;

        _delay = delay;
        _damage = damage;
        _adRatio = adRatio;
        _range = range;
        _targetTypes = targetTypes;
    }

    public override SkillData Copy()
    {
        return new MultipleShockwaveData(
            _maxUpgradePoint, // SkillData에서 상속된 값
            _waveSizeMultiply,
            _waveDelay,
            _maxWaveCount,
            _damage,
            _adRatio,
            _range,
            _delay,
            new List<ITarget.Type>(_targetTypes) // 리스트 깊은 복사
        );
    }
}


public class MultipleShockwaveCreater : SkillCreater
{
    BaseFactory _effectFactory;

    public MultipleShockwaveCreater(SkillData data, BaseFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        MultipleShockwaveData data = CopySkillData as MultipleShockwaveData;
        return new MultipleShockwave(data, _effectFactory);
    }
}
