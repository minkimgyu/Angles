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
    public enum Labels
    {
        SkillIcon,
        Life,
        Weapon,
        Effect,
        InteractableObject,
        Viewer,
        Map1
    }

    List<Tuple<string, Type>> _assetLabels;

    Dictionary<string, Sprite> _spriteAssetDictionary;
    public Dictionary<string, Sprite> SpriteAssetDictionary { get { return _spriteAssetDictionary; } }

    Dictionary<string, GameObject> _prefabAssetDictionary;
    public Dictionary<string, GameObject> PrefabAssetDictionary { get { return _prefabAssetDictionary; } }

    int labelTotalCount = 0;
    int labelCount = 0;

    private void Start()
    {
        _assetLabels = new List<Tuple<string, Type>>();
        _spriteAssetDictionary = new Dictionary<string, Sprite>();
        _prefabAssetDictionary = new Dictionary<string, GameObject>();

        _assetLabels.Add(new Tuple<string, Type>(Labels.SkillIcon.ToString(), typeof(Sprite)));
        _assetLabels.Add(new Tuple<string, Type>(Labels.Weapon.ToString(), typeof(GameObject)));
        _assetLabels.Add(new Tuple<string, Type>(Labels.Effect.ToString(), typeof(GameObject)));
        _assetLabels.Add(new Tuple<string, Type>(Labels.Life.ToString(), typeof(GameObject)));
        _assetLabels.Add(new Tuple<string, Type>(Labels.Viewer.ToString(), typeof(GameObject)));
        _assetLabels.Add(new Tuple<string, Type>(Labels.Map1.ToString(), typeof(GameObject)));

        for (int i = 0; i < _assetLabels.Count; i++)
        {
            LoadAssetAsLabel(_assetLabels[i].Item1, _assetLabels[i].Item2, 
            () => 
            {
                labelCount++;
                if (labelCount == labelTotalCount) ChangeScene();
            }
            );
        }

        Invoke("ChangeScene", 2f);
    }

    void LoadAssetAsLabel(string label, Type type, Action OnComplete)
    {
        // 빌드타겟의 경로를 가져온다.
        // 경로이기 때문에 메모리에 에셋이 로드되진 않는다.
        Addressables.LoadResourceLocationsAsync(label, type).Completed +=
        (handle) =>
        {
            IList<IResourceLocation> locationList = handle.Result;
            labelTotalCount += locationList.Count;

            if (type == typeof(Sprite))
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    LoadAsset(locationList[i], _spriteAssetDictionary);
                }
            }
            else if(type == typeof(GameObject))
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    LoadAsset(locationList[i], _prefabAssetDictionary);
                }
            }

            OnComplete?.Invoke();
        };
    }

    void LoadAsset<T>(IResourceLocation location, Dictionary<string, T> dictionary)
    {
        Addressables.LoadAssetAsync<T>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Debug.Log(location.PrimaryKey);
                    Debug.Log(handle.Result);
                    dictionary.Add(location.PrimaryKey, handle.Result);
                    break;

                case AsyncOperationStatus.Failed:
                    Debug.Log("로드 실패");
                    break;

                default:
                    break;
            }
        };
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    void ReleaseAsset()
    {
        foreach (var asset in _spriteAssetDictionary)
        {
            Addressables.Release(asset.Value);
        }

        foreach (var asset in _prefabAssetDictionary)
        {
            Addressables.Release(asset.Value);
        }
    }

    private void OnDestroy()
    {
        ReleaseAsset();
    }
}
