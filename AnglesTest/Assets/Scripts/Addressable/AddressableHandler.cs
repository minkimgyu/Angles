using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using System;

public class AddressableHandler : MonoBehaviour
{
    public enum Label
    {
        SkillIcon,
        Life,
        Weapon,
        Effect,
        InteractableObject,
        Viewer,
        Sound,
        Database,

        LevelMap,
        LevelData,

        LevelIcon,
        StatIcon,
        SkinIcon
    }

    HashSet<BaseLoader> _assetLoaders;

    int _successCount;
    int _totalCount;
    Action OnCompleted;
    Action<float> OnProgress;

    public void AddProgressEvent(Action<float> OnProgress)
    {
        this.OnProgress = OnProgress;
    }

    public AddressableHandler()
    {
        _successCount = 0;
        _totalCount = 0;
    }

    public Dictionary<SkinData.Key, Sprite> SkinIconAsset { get; private set; }
    public Dictionary<StatData.Key, Sprite> StatIconAsset { get; private set; }

    public Dictionary<GameMode.Level, Sprite> LevelIconAsset { get; private set; }

    public Dictionary<BaseSkill.Name, Sprite> SkillIconAsset { get; private set; }
    public Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabAsset { get; private set; }
    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabAsset { get; private set; }
    public Dictionary<BaseLife.Name, BaseLife> LifePrefabAsset { get; private set; }
    public Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabAsset { get; private set; }
    public Dictionary<IInteractable.Name, IInteractable> InteractableAsset { get; private set; }
    public Dictionary<ISoundPlayable.SoundName, AudioClip> SoundAsset { get; private set; }

    public Dictionary<GameMode.Level, ILevel> LevelAsset { get; private set; }
    public Dictionary<GameMode.Level, ILevelData> LevelDesignAsset { get; private set; }


    public Database Database { get; private set; }

    public void Load(Action OnCompleted)
    {
        _assetLoaders = new HashSet<BaseLoader>();


        _assetLoaders.Add(new SkinIconAssetLoader(Label.SkinIcon, (value, label) => { SkinIconAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new StatIconAssetLoader(Label.StatIcon, (value, label) => { StatIconAsset = value; OnSuccess(label); }));

        _assetLoaders.Add(new LevelIconAssetLoader(Label.LevelIcon, (value, label) => { LevelIconAsset = value; OnSuccess(label); }));

        _assetLoaders.Add(new SkillIconAssetLoader(Label.SkillIcon, (value, label) => { SkillIconAsset = value; OnSuccess(Label.SkillIcon); }));
        _assetLoaders.Add(new WeaponAssetLoader(Label.Weapon, (value, label) => { WeaponPrefabAsset = value; OnSuccess(Label.Weapon); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectPrefabAsset = value; OnSuccess(Label.Effect); }));
        _assetLoaders.Add(new LifeAssetLoader(Label.Life, (value, label) => { LifePrefabAsset = value; OnSuccess(Label.Life); }));
        _assetLoaders.Add(new ViewerAssetLoader(Label.Viewer, (value, label) => { ViewerPrefabAsset = value; OnSuccess(Label.Viewer); }));
        _assetLoaders.Add(new InteractableAssetLoader(Label.InteractableObject, (value, label) => { InteractableAsset = value; OnSuccess(Label.InteractableObject); }));
        _assetLoaders.Add(new SoundAssetLoader(Label.Sound, (value, label) => { SoundAsset = value; OnSuccess(Label.Sound); }));
        _assetLoaders.Add(new DBJsonAssetLoader(Label.Database, (value, label) => { Database = value; OnSuccess(Label.Database); }));

        // GetLevels로 가져와서 자동화 진행
        // 로드 성공을 Levels 개수 만큼 하면 됨
        _assetLoaders.Add(new LevelAssetLoader(
            Label.LevelMap,
            (value, label) => {
                LevelAsset = value;
                OnSuccess(label);
            }
        ));

        _assetLoaders.Add(new LevelJsonAssetLoader(
            Label.LevelData,
            (value, label) => {
                LevelDesignAsset = value; 
                OnSuccess(Label.LevelData); 
            }
        ));

        this.OnCompleted = OnCompleted;
        _totalCount = _assetLoaders.Count;
        foreach (var loader in _assetLoaders)
        {
            loader.Load();
        }
    }

    void OnSuccess(Label label)
    {
        _successCount++;
        Debug.Log(_successCount);
        Debug.Log(label.ToString() + "Success");

        OnProgress?.Invoke((float)_successCount / _totalCount);
        if (_successCount == _totalCount)
        {
            Debug.Log("Complete!");
            OnCompleted?.Invoke();
        }
    }

    public void Release()
    {
        foreach (var loader in _assetLoaders)
        {
            loader.Release();
        }
    }
}
