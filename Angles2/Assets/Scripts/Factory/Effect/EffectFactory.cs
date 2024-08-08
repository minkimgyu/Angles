using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCreater
{
    BaseEffect _effectPrefab;

    public EffectCreater(BaseEffect effectPrefab)
    {
        _effectPrefab = effectPrefab;
    }

    public BaseEffect Create()
    {
        BaseEffect effect = Object.Instantiate(_effectPrefab);
        if (effect == null) return null;

        effect.Initialize();
        return effect;
    }
}

public class EffectFactory
{
    Dictionary<BaseEffect.Name, EffectCreater> _effectCreaters;

    public EffectFactory(Dictionary<BaseEffect.Name, BaseEffect> effectPrefabs)
    {
        _effectCreaters = new Dictionary<BaseEffect.Name, EffectCreater>();

        int effectCount = System.Enum.GetValues(typeof(BaseEffect.Name)).Length;
        for (int i = 0; i < effectCount; i++)
        {
            BaseEffect.Name key = (BaseEffect.Name)i;
            BaseEffect prefab = effectPrefabs[key];

            _effectCreaters[key] = new EffectCreater(prefab);
        }
    }

    public BaseEffect Create(BaseEffect.Name name)
    {
        return _effectCreaters[name].Create();
    }
}