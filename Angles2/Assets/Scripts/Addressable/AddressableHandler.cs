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

        ChapterIcon,
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

        ChapterMapAsset = new Dictionary<DungeonChapter, Dictionary<BaseStage.Type, List<BaseStage>>>();
        foreach (DungeonChapter chapter in Enum.GetValues(typeof(DungeonChapter)))
        {
            ChapterMapAsset[chapter] = new Dictionary<BaseStage.Type, List<BaseStage>>();
        }
    }

    public Dictionary<DungeonChapter, Sprite> ChapterIconAsset { get; private set; }
    public Dictionary<BaseSkill.Name, Sprite> SkillIconAsset { get; private set; }
    public Dictionary<BaseWeapon.Name, BaseWeapon> WeaponPrefabAsset { get; private set; }
    public Dictionary<BaseEffect.Name, BaseEffect> EffectPrefabAsset { get; private set; }
    public Dictionary<BaseLife.Name, BaseLife> LifePrefabAsset { get; private set; }
    public Dictionary<BaseViewer.Name, BaseViewer> ViewerPrefabAsset { get; private set; }
    public Dictionary<IInteractable.Name, IInteractable> InteractableAsset { get; private set; }
    public Dictionary<ISoundPlayable.SoundName, AudioClip> SoundAsset { get; private set; }

    public Dictionary<DungeonChapter, Dictionary<BaseStage.Type, List<BaseStage>>> ChapterMapAsset { get; private set; }

    public void Load(Action OnCompleted)
    {
        _assetLoaders = new HashSet<BaseLoader>();

        _assetLoaders.Add(new ChapterIconAssetLoader(Label.ChapterIcon, (value, label) => { ChapterIconAsset = value; OnSuccess(label); }));
        _assetLoaders.Add(new SkillIconAssetLoader(Label.SkillIcon, (value, label) => { SkillIconAsset = value; OnSuccess(Label.SkillIcon); }));
        _assetLoaders.Add(new WeaponAssetLoader(Label.Weapon, (value, label) => { WeaponPrefabAsset = value; OnSuccess(Label.Weapon); }));
        _assetLoaders.Add(new EffectAssetLoader(Label.Effect, (value, label) => { EffectPrefabAsset = value; OnSuccess(Label.Effect); }));
        _assetLoaders.Add(new LifeAssetLoader(Label.Life, (value, label) => { LifePrefabAsset = value; OnSuccess(Label.Life); }));
        _assetLoaders.Add(new ViewerAssetLoader(Label.Viewer, (value, label) => { ViewerPrefabAsset = value; OnSuccess(Label.Viewer); }));
        _assetLoaders.Add(new InteractableAssetLoader(Label.InteractableObject, (value, label) => { InteractableAsset = value; OnSuccess(Label.InteractableObject); }));
        _assetLoaders.Add(new SoundAssetLoader(Label.Sound, (value, label) => { SoundAsset = value; OnSuccess(Label.Sound); }));

        _assetLoaders.Add(new MapAssetLoader(
            Label.TriconChapterMap,
            (value, label) => {
                ChapterMapAsset[DungeonChapter.TriconChapter] = value;
                OnSuccess(label);
            }
        ));

        _assetLoaders.Add(new MapAssetLoader(
            Label.RhombusChapterMap, 
            (value, label) => { ChapterMapAsset[DungeonChapter.RhombusChapter] = value; 
            OnSuccess(Label.RhombusChapterMap); 
            }
        ));

        _assetLoaders.Add(new MapAssetLoader(
            Label.PentagonicChapterMap,
            (value, label) => {
                ChapterMapAsset[DungeonChapter.PentagonicChapter] = value;
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
