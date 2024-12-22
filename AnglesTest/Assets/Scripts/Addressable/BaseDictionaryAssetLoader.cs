using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;

abstract public class BaseLoader
{
    protected AddressableHandler.Label _label;
    protected int _successCount;
    protected int _totalCount;

    protected virtual void OnSuccess()
    {
        _successCount++;
    }

    public abstract void Load();
    public abstract void Release();
}

abstract public class BaseAssetLoader<Value, Type> : BaseLoader
{
    Action<Value, AddressableHandler.Label> OnComplete;
    protected Value _asset;

    public BaseAssetLoader(AddressableHandler.Label label, Action<Value, AddressableHandler.Label> OnComplete)
    {
        _label = label;
        this.OnComplete = OnComplete;
    }

    public override void Load()
    {
        Addressables.LoadAssetAsync<Type>(_label.ToString()).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    LoadAsset(handle.Result);
                    OnComplete?.Invoke(_asset, _label);
                    break;
                case AsyncOperationStatus.Failed:
                    break;
                default:
                    break;
            }
        };
    }

    protected override void OnSuccess()
    {
        base.OnSuccess();
        if (_successCount == 1)
        {
            Debug.Log("Success");
            OnComplete?.Invoke(_asset, _label);
        }
    }

    abstract protected void LoadAsset(Type value);

    public override void Release()
    {
        Addressables.Release(_asset);
    }
}
abstract public class BaseDictionaryAssetLoader<Key, Value, Type> : BaseLoader
{
    protected Dictionary<Key, Value> _assetDictionary;
    Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete;

    public BaseDictionaryAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete)
    {
        _label = label;
        this.OnComplete = OnComplete;
        _assetDictionary = new Dictionary<Key, Value>();
        _successCount = 0;
    }

    public override void Load()
    {
        Addressables.LoadResourceLocationsAsync(_label.ToString(), typeof(Type)).Completed +=
        (handle) =>
        {
            IList<IResourceLocation> locationList = handle.Result;
            _totalCount = locationList.Count;

            for (int i = 0; i < locationList.Count; i++)
            {
                LoadAsset(locationList[i], _assetDictionary, OnSuccess);
            };
        };
    }

    protected override void OnSuccess()
    {
        base.OnSuccess();
        if (_successCount == _totalCount)
        {
            Debug.Log("Success");
            OnComplete?.Invoke(_assetDictionary, _label);
        }
    }

    protected abstract void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete);

    public override void Release()
    {
        foreach (var asset in _assetDictionary)
        {
            Addressables.Release(asset.Value);
        }
    }
}