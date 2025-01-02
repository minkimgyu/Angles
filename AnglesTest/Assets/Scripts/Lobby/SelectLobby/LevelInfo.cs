using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelInfo
{
    [JsonIgnore] GameMode.Type Type { get; } // 레벨 타입

    [JsonIgnore] bool CanUnlockLevel { get; } // 클리어 시 해금가능 여부
    [JsonIgnore] GameMode.Level UnlockLevel { get; } // 클리어 시 해금할 레벨 이름

    [JsonIgnore] virtual int MaxLevel { get { return -1; } }  // 최대 레벨
    [JsonIgnore] virtual int TotalDuration { get { return -1; } } // 최대 생존 시간
}

public interface ISavableLevelInfo
{
    [JsonIgnore] bool NowUnlock { get; set; } // 해금 여부

    [JsonIgnore] int CompleteLevel { get; set; } // 클리어 한 레벨
    [JsonIgnore] int CompleteDuration { get; set; } // 최장 생존 시간
}

public struct LevelData
{
    private ILevelInfo _levelInfos;
    private ISavableLevelInfo _savableLevelInfos;
    private Sprite _levelSprite;

    public ILevelInfo LevelInfos { get => _levelInfos; set => _levelInfos = value; }
    public ISavableLevelInfo SavableLevelInfos { get => _savableLevelInfos; set => _savableLevelInfos = value; }
    public Sprite LevelSprite { get => _levelSprite; set => _levelSprite = value; }

    public LevelData(ILevelInfo levelInfos, ISavableLevelInfo savableLevelInfos, Sprite levelSprite)
    {
        _levelInfos = levelInfos;
        _savableLevelInfos = savableLevelInfos;
        _levelSprite = levelSprite;
    }
}

public struct ChapterInfo : ILevelInfo
{
    [JsonProperty] private GameMode.Type _type;
    [JsonProperty] bool _canUnlockLevel;
    [JsonProperty] GameMode.Level _unlockLevel;

    [JsonProperty] private int _maxLevel;

    [JsonIgnore] public GameMode.Type Type { get => _type; }
    [JsonIgnore] public bool CanUnlockLevel { get => _canUnlockLevel; }
    [JsonIgnore] public GameMode.Level UnlockLevel { get => _unlockLevel; }

    [JsonIgnore] public int MaxLevel { get => _maxLevel; }


    public ChapterInfo(int maxLevel)
    {
        _canUnlockLevel = false;
        _unlockLevel = default;

        _type = GameMode.Type.Chapter;
        _maxLevel = maxLevel;
    }

    public ChapterInfo(int maxLevel, GameMode.Level unlockLevel)
    {
        _canUnlockLevel = true;
        _unlockLevel = unlockLevel;

        _type = GameMode.Type.Chapter;
        _maxLevel = maxLevel;
    }
}

public struct SavableChapterInfo : ISavableLevelInfo
{
    [JsonProperty] private int _completeLevel;
    [JsonProperty] private bool _nowUnlock; // 이전 스테이지를 클리어해야 해금된다.

    [JsonIgnore] public int CompleteLevel { get => _completeLevel; set => _completeLevel = value; }
    [JsonIgnore] public bool NowUnlock { get => _nowUnlock; set => _nowUnlock = value; }
    [JsonIgnore] public int CompleteDuration { get { return -1; } set { } }

    public SavableChapterInfo(bool nowUnlock)
    {
        _completeLevel = 0;
        _nowUnlock = nowUnlock;
    }

    public SavableChapterInfo(int completeLevel, bool nowUnlock)
    {
        _completeLevel = completeLevel;
        _nowUnlock = nowUnlock;
    }
}

// 생존 시간 추가
public struct SurvivalInfo : ILevelInfo
{
    [JsonProperty] private GameMode.Type _type;
    [JsonProperty] bool _canUnlockLevel;
    [JsonProperty] GameMode.Level _unlockLevel;

    [JsonProperty] private int _totalDuration;

    [JsonIgnore] public GameMode.Type Type { get => _type; }
    [JsonIgnore] public bool CanUnlockLevel { get => _canUnlockLevel; }
    [JsonIgnore] public GameMode.Level UnlockLevel { get => _unlockLevel; }

    [JsonIgnore] public int TotalDuration { get => _totalDuration; }

    public SurvivalInfo(int maxDuration)
    {
        _canUnlockLevel = false;
        _unlockLevel = default;

        _type = GameMode.Type.Survival;
        _totalDuration = maxDuration;
    }

    public SurvivalInfo(int maxDuration, GameMode.Level unlockLevel)
    {
        _canUnlockLevel = true;
        _unlockLevel = unlockLevel;
        _type = GameMode.Type.Survival;

        _totalDuration = maxDuration;
    }
}

public struct SavableSurvivalInfo : ISavableLevelInfo
{
    [JsonProperty] private int _completeDuration; // 생존한 시간
    [JsonProperty] private bool _nowUnlock; // 이전 스테이지를 클리어해야 해금된다.

    [JsonIgnore] public int CompleteDuration { get => _completeDuration; set => _completeDuration = value; }
    [JsonIgnore] public bool NowUnlock { get => _nowUnlock; set => _nowUnlock = value; }
    [JsonIgnore] public int CompleteLevel { get { return -1; } set { } }

    public SavableSurvivalInfo(bool nowUnlock)
    {
        _completeDuration = 0;
        _nowUnlock = nowUnlock;
    }

    public SavableSurvivalInfo(int completeDuration, bool nowUnlock)
    {
        _completeDuration = completeDuration;
        _nowUnlock = nowUnlock;
    }
}