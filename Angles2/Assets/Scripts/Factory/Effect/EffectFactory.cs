using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectCreaterInput
{
    public BaseEffect _effectPrefab;
}

public class EffectCreater : BaseCreater<EffectCreaterInput, BaseEffect>
{
    //protected BaseEffect _prefab;

    public override void Initialize(EffectCreaterInput input)
    {
        //_prefab = input._effectPrefab;
    }

    public override BaseEffect Create() 
    {
        //BaseEffect effect = Object.Instantiate(_prefab);
        //effect.Initialize();

        //return effect; 
        return null;
    }
}

public class EffectFactory : Singleton<EffectFactory>
{
    [SerializeField] EffectInputDictionary _effectInputs; // 무기 prefab을 모아서 넣어준다.
    Dictionary<BaseEffect.Name, EffectCreater> _effectCreaters;

    private static EffectFactory _instance;

    protected override void Awake()
    {
        base.Awake();
        _effectCreaters = new Dictionary<BaseEffect.Name, EffectCreater>();
        //AddressableManager.Instance.CardIconAssetDictionary;

        Initialize();
    }

    private void Initialize()
    {
        foreach (var input in _effectInputs)
        {
            _effectCreaters[input.Key] = new EffectCreater();
            _effectCreaters[input.Key].Initialize(input.Value);
        }
    }

    public static BaseEffect Create(BaseEffect.Name name)
    {
        return _instance._effectCreaters[name].Create();
    }
}