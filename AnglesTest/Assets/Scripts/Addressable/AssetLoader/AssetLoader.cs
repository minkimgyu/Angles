using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;
using Skill;

public class SoundAssetLoader : AssetLoader<ISoundPlayable.SoundName, AudioClip, AudioClip>
{
    public SoundAssetLoader(AddressableHandler.Label label, Action<Dictionary<ISoundPlayable.SoundName, AudioClip>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class SkinIconAssetLoader : AssetLoader<SkinData.Key, Sprite, Sprite>
{
    public SkinIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<SkinData.Key, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class StatIconAssetLoader : AssetLoader<StatData.Key, Sprite, Sprite>
{
    public StatIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<StatData.Key, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class LevelIconAssetLoader : AssetLoader<GameMode.Level, Sprite, Sprite>
{
    public LevelIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<GameMode.Level, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

public class SkillIconAssetLoader : AssetLoader<BaseSkill.Name, Sprite, Sprite>
{
    public SkillIconAssetLoader(AddressableHandler.Label label, Action<Dictionary<BaseSkill.Name, Sprite>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }
}

abstract public class AssetLoader<Key, Value, Type> : MultipleAssetLoader<Key, Value, Type>
{
    protected AssetLoader(AddressableHandler.Label label, Action<Dictionary<Key, Value>, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(IResourceLocation location, Dictionary<Key, Value> dictionary, Action OnComplete)
    {
        Addressables.LoadAssetAsync<Value>(location).Completed +=
        (handle) =>
        {
            switch (handle.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Key key = (Key)Enum.Parse(typeof(Key), location.PrimaryKey);

                    dictionary.Add(key, handle.Result);
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

public class GoldIconAssetLoader : SingleAssetLoader<Sprite, Sprite>
{
    public GoldIconAssetLoader(AddressableHandler.Label label, Action<Sprite, AddressableHandler.Label> OnComplete) : base(label, OnComplete)
    {
    }

    protected override void LoadAsset(Sprite item) => _asset = item;
}