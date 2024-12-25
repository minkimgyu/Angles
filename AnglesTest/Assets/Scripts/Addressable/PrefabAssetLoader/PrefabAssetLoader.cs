using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class LifeAssetLoader : MultiplePrafabAssetLoader<BaseLife.Name, BaseLife>
{
    public LifeAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseLife.Name, BaseLife>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class WeaponAssetLoader : MultiplePrafabAssetLoader<BaseWeapon.Name, BaseWeapon>
{
    public WeaponAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseWeapon.Name, BaseWeapon>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class InteractableAssetLoader : MultiplePrafabAssetLoader<IInteractable.Name, IInteractable>
{
    public InteractableAssetLoader(AddressableHandler.Label label, Action<Dictionary<IInteractable.Name, IInteractable>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class EffectAssetLoader : MultiplePrafabAssetLoader<BaseEffect.Name, BaseEffect>
{
    public EffectAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseEffect.Name, BaseEffect>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class ViewerAssetLoader : MultiplePrafabAssetLoader<BaseViewer.Name, BaseViewer>
{
    public ViewerAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseViewer.Name, BaseViewer>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class LevelAssetLoader : MultiplePrafabAssetLoader<GameMode.Level, ILevel>
{
    public LevelAssetLoader(AddressableHandler.Label label, Action<Dictionary<GameMode.Level, ILevel>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

abstract public class MultiplePrafabAssetLoader<Key, Value> : MultipleAssetLoader<Key, Value, GameObject>
{
    protected MultiplePrafabAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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

abstract public class SinglePrafabAssetLoader<Value> : SingleAssetLoader<Value, GameObject>
{
    protected SinglePrafabAssetLoader(AddressableHandler.Label label, Action<Value, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(GameObject value)
    {
        _asset = value.GetComponent<Value>();
    }
}