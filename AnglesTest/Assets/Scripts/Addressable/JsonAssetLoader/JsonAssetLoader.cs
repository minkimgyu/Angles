using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class ChapterMapJsonAssetLoader : DictionaryJsonAssetLoader<BaseStage.Name, IStageData>
{
    public ChapterMapJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseStage.Name, IStageData>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class JsonAssetLoader<Value> : BaseAssetLoader<Value, TextAsset>
{
    JsonParser _parser;
    public JsonAssetLoader(AddressableHandler.Label label, Action<Value, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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

abstract public class DictionaryJsonAssetLoader<Key, Value> : BaseDictionaryAssetLoader<Key, Value, TextAsset>
{
    JsonParser _parser;
    protected DictionaryJsonAssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
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

