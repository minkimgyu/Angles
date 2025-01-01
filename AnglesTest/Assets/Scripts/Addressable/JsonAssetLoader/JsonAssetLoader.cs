using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class SingleJsonAssetLoader<Value> : SingleAssetLoader<Value, TextAsset>
{
    JsonParser _parser;
    public SingleJsonAssetLoader(AddressableHandler.Label label, Action<Value, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
        _parser = new JsonParser();
    }

    protected override void LoadAsset(TextAsset value)
    {
        //byte[] bytes = System.Convert.FromBase64String(value.text);
        //string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);
        //_asset = _parser.JsonToObject<Value>(decodedJson);
        _asset = _parser.JsonToObject<Value>(value.text);
    }
}

public class MultipleJsonAssetLoader<Key, Value> : MultipleAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    public MultipleJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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

public class LevelJsonAssetLoader : MultipleJsonAssetLoader<GameMode.Level, ILevelData>
{
    public LevelJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<GameMode.Level, ILevelData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class DBJsonAssetLoader : SingleJsonAssetLoader<Database>
{
    public DBJsonAssetLoader(AddressableHandler.Label label, Action<Database, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class LocalizationJsonAssetLoader : SingleJsonAssetLoader<Localization>
{
    public LocalizationJsonAssetLoader(AddressableHandler.Label label, Action<Localization, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}