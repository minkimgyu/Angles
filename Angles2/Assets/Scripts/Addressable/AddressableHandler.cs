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

        TriconChapterMap,
        RhombusChapterMap,
        PentagonicChapterMap,

        TriconChapterData,
        RhombusChapterData,
        PentagonicChapterData,

        ChapterIcon,
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

        ChapterMapAsset = new Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, BaseStage>>();
        foreach (DungeonMode.Chapter chapter in Enum.GetValues(typeof(DungeonMode.Chapter)))
        {
            ChapterMapAsset[chapter] = new Dictionary<BaseStage.Name, BaseStage>();
        }

        ChapterMapLevelDesignAsset = new Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, IStageData>>();
        foreach (DungeonMode.Chapter chapter in Enum.GetValues(typeof(DungeonMode.Chapter)))
        {
            ChapterMapLevelDesignAsset[chapter] = new Dictionary<BaseStage.Name, IStageData>();
        }
    }

    public Dictionary<SkinData.Key, Sprite> SkinIconAsset { get; private set; }
    public Dictionary<StatData.Key, Sprite> StatIconAsset { get; private set; }
    public Dictionary<DungeonMode.Chapter, Sprite> ChapterIconAsset { get; private set; }
    public Dictionary<BaseSkill.Name, Sprite> SkillIconAsset { get; private set; }
    public Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabAsset { get; private set; }
    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabAsset { get; private set; }
    public Dictionary<BaseLife.Name, BaseLife> LifePrefabAsset { get; private set; }
    public Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabAsset { get; private set; }
    public Dictionary<IInteractable.Name, IInteractable> InteractableAsset { get; private set; }
    public Dictionary<ISoundPlayable.SoundName, AudioClip> SoundAsset { get; private set; }

    public Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, BaseStage>> ChapterMapAsset { get; private set; }
    public Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, IStageData>> ChapterMapLevelDesignAsset { get; private set; }

    public void Load(Action OnCompleted)
    {
        _assetLoaders = new HashSet<BaseLoader>();

        _assetLoaders.Add(new SkinIconAssetLoader(Label.SkinIcon, (value, label) => { SkinIconAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new StatIconAssetLoader(Label.StatIcon, (value, label) => { StatIconAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new ChapterIconAssetLoader(Label.ChapterIcon, (value, label) => { ChapterIconAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new SkillIconAssetLoader(Label.SkillIcon, (value, label) => { SkillIconAsset = value; OnSuccess(Label.SkillIcon); }));
        _assetLoaders.Add(new WeaponAssetLoader(Label.Weapon, (value, label) => { WeaponPrefabAsset = value; OnSuccess(Label.Weapon); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectPrefabAsset = value; OnSuccess(Label.Effect); }));
        _assetLoaders.Add(new LifeAssetLoader(Label.Life, (value, label) => { LifePrefabAsset = value; OnSuccess(Label.Life); }));
        _assetLoaders.Add(new ViewerAssetLoader(Label.Viewer, (value, label) => { ViewerPrefabAsset = value; OnSuccess(Label.Viewer); }));
        _assetLoaders.Add(new InteractableAssetLoader(Label.InteractableObject, (value, label) => { InteractableAsset = value; OnSuccess(Label.InteractableObject); }));
        _assetLoaders.Add(new SoundAssetLoader(Label.Sound, (value, label) => { SoundAsset = value; OnSuccess(Label.Sound); }));



        _assetLoaders.Add(new ChapterMapJsonAssetLoader(
            Label.TriconChapterData,
            (value, label) => {
                ChapterMapLevelDesignAsset[DungeonMode.Chapter.TriconChapter] = value; 
                OnSuccess(Label.TriconChapterData); 
            }
        ));

        _assetLoaders.Add(new ChapterMapJsonAssetLoader(
            Label.RhombusChapterData,
            (value, label) => {
                ChapterMapLevelDesignAsset[DungeonMode.Chapter.RhombusChapter] = value;
                OnSuccess(Label.RhombusChapterData);
            }
        ));

        _assetLoaders.Add(new ChapterMapJsonAssetLoader(
            Label.PentagonicChapterData,
            (value, label) => {
                ChapterMapLevelDesignAsset[DungeonMode.Chapter.PentagonicChapter] = value;
                OnSuccess(Label.PentagonicChapterData);
            }
        ));




        _assetLoaders.Add(new ChapterMapAssetLoader(
            Label.TriconChapterMap,
            (value, label) => {
                ChapterMapAsset[DungeonMode.Chapter.TriconChapter] = value;
                OnSuccess(label);
            }
        ));

        _assetLoaders.Add(new ChapterMapAssetLoader(
            Label.RhombusChapterMap, 
            (value, label) => { ChapterMapAsset[DungeonMode.Chapter.RhombusChapter] = value; 
            OnSuccess(Label.RhombusChapterMap); 
            }
        ));

        _assetLoaders.Add(new ChapterMapAssetLoader(
            Label.PentagonicChapterMap,
            (value, label) => {
                ChapterMapAsset[DungeonMode.Chapter.PentagonicChapter] = value;
                OnSuccess(label);
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
