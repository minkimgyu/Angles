using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class LifeJsonAssetLoader : JsonAssetLoader<BaseLife.Name, LifeData>
{
    public LifeJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseLife.Name, LifeData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}
public class WeaponJsonAssetLoader : JsonAssetLoader<BaseWeapon.Name, WeaponData>
{
    public WeaponJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseWeapon.Name, WeaponData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

//public class InteractableJsonAssetLoader : JsonAssetLoader<IInteractable.Name, IInteractable>
//{
//    public InteractableJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<IInteractable.Name, IInteractable>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
//    {
//    }
//}

public class ChapterMapJsonAssetLoader : JsonAssetLoader<BaseStage.Name, IStageData>
{
    public ChapterMapJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseStage.Name, IStageData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}


abstract public class JsonAssetLoader<Key, Value> : BaseDictionaryAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    protected JsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<TextAsset>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = _parser.JsonToObject<Value>(handle.Result);

                    dictionary.Add(key, value);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    break;

                default:
                    break;
            }
        };
    }
}

