using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

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

abstract public class BaseDictionaryListAssetLoader<Key, Value, Type> : BaseLoader
{
    protected Dictionary<Key, List<Value>> _assetDictionary;
    Action<Dictionary<Key, List<Value>>, AddressableHandler.Label> OnComplete;

    public BaseDictionaryListAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, List<Value>>, AddressableHandler.Label> OnComplete)
    {
        _label = label;
        this.OnComplete = OnComplete;
        _assetDictionary = new Dictionary<Key, List<Value>>();

        foreach (Key key in Enum.GetValues(typeof(Key)))
        {
            _assetDictionary[key] = new List<Value>();
        }
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

    public override void Release()
    {
        foreach (var asset in _assetDictionary)
        {
            Addressables.Release(asset);
        }
    }

    protected override void OnSuccess()
    {
        base.OnSuccess();
        if (_successCount == _totalCount) // 개수 조정해줘야함 타입 개수 * 에셋 개수
        {
            Debug.Log("Success");
            OnComplete?.Invoke(_assetDictionary, _label);
        }
    }

    protected abstract void LoadAsset(IResourceLocation location, Dictionary<Key, List<Value>> asset, Action OnComplete);
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