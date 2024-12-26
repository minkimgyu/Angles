using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

abstract public class StageCreater
{
    public abstract ILevel Create();
}

public class ChapterLevelCreater : StageCreater
{
    BossStageData _bossStageData;
    MobStageData[] _mobStageDatas;

    ILevel _chapterStageCollection;

    public ChapterLevelCreater(
        ILevel chapterStageCollection,
        BossStageData bossStageData,
        MobStageData[] mobStageDatas)
    {
        _chapterStageCollection = chapterStageCollection;
        _bossStageData = bossStageData;
        _mobStageDatas = mobStageDatas;
    }

    public override ILevel Create()
    {
        ILevel level = Object.Instantiate(_chapterStageCollection as ChapterStageCollection);
        if (level == null) return null;

        level.BossStage.ResetData(_bossStageData);
        for (int i = 0; i < _mobStageDatas.Length; i++)
        {
            level.MobStages[i].ResetData(_mobStageDatas[i]);
        }

        return level;
    }
}

public class ChapterStageFactory : BaseFactory
{
    Dictionary<GameMode.Level, StageCreater> _stageCreaters;

    public ChapterStageFactory(Dictionary<GameMode.Level, ILevel> stagePrefab, Dictionary<GameMode.Level, ILevelData> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, StageCreater>();

        List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < levels.Count; i++)
        {
            _stageCreaters[levels[i]] = new ChapterLevelCreater(
                stagePrefab[levels[i]],
                stageData[levels[i]].BossStageData,
                stageData[levels[i]].MobStageDatas
            );
        }
    }

    public override ILevel Create(GameMode.Level level)
    {
        return _stageCreaters[level].Create();
    }
}