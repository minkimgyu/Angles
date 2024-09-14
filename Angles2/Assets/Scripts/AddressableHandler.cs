using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

//public interface IAddressableHandler
//{
//    Dictionary<BaseSkill.Name, Sprite> SkillIcons { get; }
//    Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabs { get; }
//    Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabs { get; }
//    Dictionary<BaseLife.Name, BaseLife> LifePrefabs { get; }
//    Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabs { get; }
//    Dictionary<IInteractable.Name, IInteractable> InteractableAssetDictionary { get; }
//    Dictionary<ISoundPlayable.SoundName, AudioClip> AudioAssetDictionary { get; }

//    List<BaseStage> StartStageAssetDictionary { get; }
//    List<BaseStage> BonusStageAssetDictionary { get; }
//    List<BaseStage> BattleStageAssetDictionary { get; }

//    void Load(Action OnCompleted);
//    public void Release();
//}

//public class NullAddressableHandler : IAddressableHandler
//{
//    public Dictionary<BaseSkill.Name, Sprite> SkillIcons { get { return new Dictionary<BaseSkill.Name, Sprite>(); } }
//    public Dictionary<ISoundPlayable.SoundName, AudioClip> AudioAssetDictionary { get { return new Dictionary<ISoundPlayable.SoundName, AudioClip>(); } }

//    public Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabs { get { return new Dictionary<BaseWeapon.Name, BaseWeapon>(); } }

//    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabs { get { return new Dictionary<BaseEffect.Name, BaseEffect>(); } }

//    public Dictionary<BaseLife.Name, BaseLife> LifePrefabs { get { return new Dictionary<BaseLife.Name, BaseLife>(); } }

//    public Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabs { get { return new Dictionary<BaseViewer.Name, BaseViewer>(); } }

//    public Dictionary<IInteractable.Name, IInteractable> InteractableAssetDictionary { get { return new Dictionary<IInteractable.Name, IInteractable>(); } }


//    public List<BaseStage> StartStageAssetDictionary { get { return new List<BaseStage>(); } }

//    public List<BaseStage> BonusStageAssetDictionary { get { return new List<BaseStage>(); } }

//    public List<BaseStage> BattleStageAssetDictionary { get { return new List<BaseStage>(); } }

//    public void Load(Action OnCompleted) { }
//    public void Release() { }
//}

public class AddressableHandler : MonoBehaviour
{
    public enum Labels
    {
        SkillIcon,
        Life,
        Weapon,
        Effect,
        InteractableObject,
        Viewer,
        Sound,

        StartStage,
        BonusStage,
        BattleStage
    }

    List<Tuple<Labels, Type>> _assetLabels;

    Dictionary<BaseSkill.Name, Sprite> _skillIconAssetDictionary;
    public Dictionary<BaseSkill.Name, Sprite> SkillIcons { get { return _skillIconAssetDictionary; } }

    Dictionary<BaseWeapon.Name, BaseWeapon> _weaponAssetDictionary;
    public Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabs { get { return _weaponAssetDictionary; } }

    Dictionary<BaseEffect.Name, BaseEffect> _effectAssetDictionary;
    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabs { get { return _effectAssetDictionary; } }

    Dictionary<BaseLife.Name, BaseLife> _lifeAssetDictionary;
    public Dictionary<BaseLife.Name, BaseLife> LifePrefabs { get { return _lifeAssetDictionary; } }

    Dictionary<BaseViewer.Name, BaseViewer> _viewerAssetDictionary;
    public Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabs { get { return _viewerAssetDictionary; } }

    Dictionary<IInteractable.Name, IInteractable> _interactableAssetDictionary;
    public Dictionary<IInteractable.Name, IInteractable> InteractableAssetDictionary { get { return _interactableAssetDictionary; } }

    Dictionary<ISoundPlayable.SoundName, AudioClip> _audioAssetDictionary;
    public Dictionary<ISoundPlayable.SoundName, AudioClip> AudioAssetDictionary { get { return _audioAssetDictionary; } }


    List<BaseStage> _startStageAssetList;
    public List<BaseStage> StartStageAssetDictionary { get { return _startStageAssetList; } }

    List<BaseStage> _bonusStageAssetList;
    public List<BaseStage> BonusStageAssetDictionary { get { return _bonusStageAssetList; } }

    List<BaseStage> _battleStageAssetList;
    public List<BaseStage> BattleStageAssetDictionary { get { return _battleStageAssetList; } }

    int labelCount = 0;

    public void Load(Action OnCompleted)
    {
        _assetLabels = new List<Tuple<Labels, Type>>();
        _skillIconAssetDictionary = new Dictionary<BaseSkill.Name, Sprite>();
        _weaponAssetDictionary = new Dictionary<BaseWeapon.Name, BaseWeapon>();
        _effectAssetDictionary = new Dictionary<BaseEffect.Name, BaseEffect>();
        _lifeAssetDictionary = new Dictionary<BaseLife.Name, BaseLife>();
        _viewerAssetDictionary = new Dictionary<BaseViewer.Name, BaseViewer>();
        _interactableAssetDictionary = new Dictionary<IInteractable.Name, IInteractable>();
        _audioAssetDictionary = new Dictionary<ISoundPlayable.SoundName, AudioClip>();

        _startStageAssetList = new List<BaseStage>();
        _bonusStageAssetList = new List<BaseStage>();
        _battleStageAssetList = new List<BaseStage>();

        _assetLabels.Add(new Tuple<Labels, Type> (Labels.SkillIcon, typeof(Sprite)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.Weapon, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.Effect, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.Life, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.Viewer, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.InteractableObject, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.Sound, typeof(AudioClip)));

        _assetLabels.Add(new Tuple<Labels, Type> (Labels.StartStage, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.BonusStage, typeof(GameObject)));
        _assetLabels.Add(new Tuple<Labels, Type> (Labels.BattleStage, typeof(GameObject)));

        for (int i = 0; i < _assetLabels.Count; i++)
        {
            LoadAssetAsLabel(_assetLabels[i].Item1, _assetLabels[i].Item2,
            () => 
            {
                labelCount++;
                //Debug.Log(labelCount);
                if (labelCount == _assetLabels.Count)
                {
                    OnCompleted?.Invoke();
                }
            }
            );
        }
    }

    public void Release()
    {
        ReleaseAsset();
    }

    void LoadAssetAsLabel(Labels label, Type assetType, Action OnComplete)
    {
        // 빌드타겟의 경로를 가져온다.
        Debug.Log(assetType);

        Addressables.LoadResourceLocationsAsync(label.ToString(), assetType).Completed +=
        (handle) =>
        {
            IList<IResourceLocation> locationList = handle.Result;
            int locationCount = locationList.Count;

            // --> AssetLoader를 만들어서 Factory 패턴과 유사하게 만들어도 괜찮을 듯?
            for (int i = 0; i < locationCount; i++)
            {
                switch (label)
                {
                    case Labels.SkillIcon:
                        LoadAsset(locationList[i], _skillIconAssetDictionary, () => { if (locationCount == _skillIconAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.Life:
                        LoadPrefab(locationList[i], _lifeAssetDictionary, () => { if (locationCount == _lifeAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.Weapon:
                        LoadPrefab(locationList[i], _weaponAssetDictionary, () => { if (locationCount == _weaponAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.Effect:
                        LoadPrefab(locationList[i], _effectAssetDictionary, () => { if (locationCount == _effectAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.Viewer:
                        LoadPrefab(locationList[i], _viewerAssetDictionary, () => { if (locationCount == _viewerAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.InteractableObject:
                        LoadPrefab(locationList[i], _interactableAssetDictionary, () => { if (locationCount == _interactableAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.Sound:
                        LoadAsset(locationList[i], _audioAssetDictionary, () => { if (locationCount == _audioAssetDictionary.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.StartStage:
                        LoadPrefab(locationList[i], _startStageAssetList, () => { if (locationCount == _startStageAssetList.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.BonusStage:
                        LoadPrefab(locationList[i], _bonusStageAssetList, () => { if (locationCount == _bonusStageAssetList.Count) OnComplete?.Invoke(); });
                        break;
                    case Labels.BattleStage:
                        LoadPrefab(locationList[i], _battleStageAssetList, () => { if (locationCount == _battleStageAssetList.Count) OnComplete?.Invoke(); });
                        break;
                    default:
                        break;
                }
            }
        };
    }

    void LoadAsset<T1, T2>(IResourceLocation location, Dictionary<T1, T2> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<T2>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    T1 key = (T1)Enum.Parse(typeof(T1), location.PrimaryKey);

                    dictionary.Add(key, handle.Result);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    Debug.Log("로드 실패");
                    break;

                default:
                    break;
            }
        };
    }

    void LoadPrefab<T1, T2>(IResourceLocation location, Dictionary<T1, T2> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    T1 key = (T1)Enum.Parse(typeof(T1), location.PrimaryKey);
                    T2 value = handle.Result.GetComponent<T2>();

                    //Debug.Log(key);
                    //Debug.Log(handle.Result);

                    dictionary.Add(key, value);
                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    Debug.Log("로드 실패");
                    break;

                default:
                    break;
            }
        };
    }

    void LoadPrefab<T1>(IResourceLocation location, List<T1> list, Action OnComplete)
    {
        Addressables.LoadAssetAsync<GameObject>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    T1 value = handle.Result.GetComponent<T1>();
                    list.Add(value);

                    OnComplete?.Invoke();
                    break;

                case AsyncOperationStatus.Failed:
                    Debug.Log("로드 실패");
                    break;

                default:
                    break;
            }
        };
    }

    void ReleaseAsset()
    {
        foreach (var asset in _skillIconAssetDictionary)
        {
            Addressables.Release(asset.Value);
        }

        foreach (var asset in _lifeAssetDictionary)
        {
            Addressables.Release(asset.Value.gameObject);
        }

        foreach (var asset in _weaponAssetDictionary)
        {
            Addressables.Release(asset.Value.gameObject);
        }

        foreach (var asset in _effectAssetDictionary)
        {
            Addressables.Release(asset.Value.gameObject);
        }

        foreach (var asset in _viewerAssetDictionary)
        {
            Addressables.Release(asset.Value.gameObject);
        }

        foreach (var asset in _interactableAssetDictionary)
        {
            Addressables.Release(asset.Value.ReturnGameObject());
        }
    }
}
