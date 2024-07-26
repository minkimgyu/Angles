using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCreater : ObjCreater<BaseEffect> 
{
    public override BaseEffect Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseEffect effect = obj.GetComponent<BaseEffect>();
        if (effect == null) return null;

        effect.Initialize();
        return effect;
    }
}

public class EffectFactory : MonoBehaviour
{
    Dictionary<BaseEffect.Name, EffectCreater> _effectCreaters;
    private static EffectFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _effectCreaters = new Dictionary<BaseEffect.Name, EffectCreater>();

        int effectCount = System.Enum.GetValues(typeof(BaseEffect.Name)).Length;
        for (int i = 0; i < effectCount; i++)
        {
            BaseEffect.Name key = (BaseEffect.Name)i;
            GameObject prefab = AddressableManager.Instance.PrefabAssetDictionary[key.ToString()];

            _effectCreaters[key] = new EffectCreater();
            _effectCreaters[key].Initialize(prefab);
        }
    }

    public static BaseEffect Create(BaseEffect.Name name)
    {
        return _instance._effectCreaters[name].Create();
    }
}