using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultipleShockwaveData : SkillData
{
    public float _waveSizeMultiply;
    public float _waveDelay;
    public int _maxWaveCount;

    public float _delay;
    public float _damage;
    public float _range;
    public List<ITarget.Type> _targetTypes;

    public MultipleShockwaveData(int maxUpgradePoint, float waveSizeMultiply, float waveDelay, int maxWaveCount, float damage, float range, float delay, List<ITarget.Type> targetTypes) : base(maxUpgradePoint)
    {
        _waveSizeMultiply = waveSizeMultiply;
        _waveDelay = waveDelay;
        _maxWaveCount = maxWaveCount;

        _delay = delay;
        _damage = damage;
        _range = range;
        _targetTypes = targetTypes;
    }
}


public class MultipleShockwaveCreater : SkillCreater
{
    EffectFactory _effectFactory;

    public MultipleShockwaveCreater(SkillData data, EffectFactory effectFactory) : base(data)
    {
        _effectFactory = effectFactory;
    }

    public override BaseSkill Create()
    {
        MultipleShockwaveData data = _skillData as MultipleShockwaveData;
        return new MultipleShockwave(data, _effectFactory);
    }
}
