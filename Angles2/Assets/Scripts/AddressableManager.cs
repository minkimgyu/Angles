using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

public class AddressableManager : Singleton<AddressableManager>
{
    Dictionary<string, AsyncOperationHandle<Sprite>> _spriteAssetDictionary;
    public Dictionary<string, AsyncOperationHandle<Sprite>> SpriteAssetDictionary { get { return _spriteAssetDictionary; } }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _spriteAssetDictionary = new Dictionary<string, AsyncOperationHandle<Sprite>>();

        int count = Enum.GetValues(typeof(CardData.Name)).Length;
        foreach (CardData.Name name in Enum.GetValues(typeof(CardData.Name)))
        {
            LoadAssetAsName<Sprite>(name.ToString(), (item) => 
            {
                _spriteAssetDictionary.Add(name.ToString(), item);

                if (_spriteAssetDictionary.Count != count) return;
                ChangeScene();
            });
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    void LoadAssetAsName<T>(string name, Action<AsyncOperationHandle<T>> ReturnAssets)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);
        handle.Completed += ReturnAssets;
    }

    void ReleaseAsset()
    {
        foreach (var asset in _spriteAssetDictionary)
        {
            Addressables.Release(asset.Value.Result);
        }
    }

    private void OnDestroy()
    {
        ReleaseAsset();
    }

    //void LoadAsset<T>(IResourceLocation location, Action<AsyncOperationHandle<T>> ReturnAssets)
    //{
    //    // 해당 위치에 있는 스프라이트를 로드한다.
    //    Addressables.LoadAssetAsync<T>(location).Completed += ReturnAssets;
    //}

    //void LoadAssetAsLabel<T>(string label, Action<AsyncOperationHandle<T>> ReturnAssets)
    //{
    //    Addressables.LoadResourceLocationsAsync(label, typeof(T)).Completed +=
    //    (handle) => 
    //    {
    //        for (int i = 0; i < handle.Result.Count; i++)
    //        {
    //            LoadAsset(handle.Result[i], ReturnAssets);
    //        }
    //    };
    //}
}
