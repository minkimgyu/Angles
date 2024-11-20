using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class LifeAssetLoader : PrafabAssetLoader<BaseLife.Name, BaseLife>
{
    public LifeAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseLife.Name, BaseLife>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class WeaponAssetLoader : PrafabAssetLoader<BaseWeapon.Name, BaseWeapon>
{
    public WeaponAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseWeapon.Name, BaseWeapon>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class InteractableAssetLoader : PrafabAssetLoader<IInteractable.Name, IInteractable>
{
    public InteractableAssetLoader(AddressableHandler.Label label, Action<Dictionary<IInteractable.Name, IInteractable>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class EffectAssetLoader : PrafabAssetLoader<BaseEffect.Name, BaseEffect>
{
    public EffectAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseEffect.Name, BaseEffect>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ViewerAssetLoader : PrafabAssetLoader<BaseViewer.Name, BaseViewer>
{
    public ViewerAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseViewer.Name, BaseViewer>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ChapterMapAssetLoader : PrafabAssetLoader<BaseStage.Name, BaseStage>
{
    public ChapterMapAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseStage.Name, BaseStage>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

abstract public class PrafabAssetLoader<Key, Value> : BaseDictionaryAssetLoader<Key, Value, GameObject>
{
    protected PrafabAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);
                    Value value = handle.Result.GetComponent<Value>();

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