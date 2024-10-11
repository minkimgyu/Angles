using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class MapAssetLoader : BaseDictionaryListAssetLoader<BaseStage.Type, BaseStage, GameObject>
{
    public MapAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseStage.Type, List<BaseStage>>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {

    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<BaseStage.Type, List<BaseStage>> asset, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:

                    // key�� �����ϴ� ���� ã�ƾ��Ѵ�.
                    BaseStage value = handle.Result.GetComponent<BaseStage>();

                    foreach (BaseStage.Type stageType in Enum.GetValues(typeof(BaseStage.Type)))
                    {
                        if (location.PrimaryKey.Contains(stageType.ToString()))
                        {
                            List<BaseStage> valueList = new List<BaseStage>();



                            _assetDictionary[stageType].Add(value); // ã�Ƽ� ���� �� break ����
                            break;
                        }
                    }

                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    Debug.Log("�ε� ����");
                    break;

                default:
                    break;
            }
        };
    }
}