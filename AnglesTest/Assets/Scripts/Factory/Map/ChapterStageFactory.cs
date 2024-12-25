using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

abstract public class StageCreater
{
    protected BaseStage _stagePrefab;

    public StageCreater(BaseStage stagePrefab)
    {
        _stagePrefab = stagePrefab;
    }

    public abstract BaseStage Create();
}

public class NormalStageCreater : StageCreater
{
    public NormalStageCreater(BaseStage stagePrefab) : base(stagePrefab) { }

    public override BaseStage Create()
    {
        BaseStage stage = Object.Instantiate(_stagePrefab);
        return stage;
    }
}

public class BossStageCreater : StageCreater
{
    BossStageData _bossStageData;

    public BossStageCreater(BaseStage stagePrefab, BossStageData bossStageData) : base(stagePrefab)
    {
        _bossStageData = bossStageData;
    }

    public override BaseStage Create()
    {
        BaseStage stage = Object.Instantiate(_stagePrefab);
        if (stage == null) return null;

        stage.ResetData(_bossStageData);
        return stage;
    }
}

public class MobStageCreater : StageCreater
{
    MobStageData _mobStageData;

    public MobStageCreater(BaseStage stagePrefab, MobStageData mobStageData) : base(stagePrefab) 
    {
        _mobStageData = mobStageData;
    }

    public override BaseStage Create()
    {
        BaseStage stage = Object.Instantiate(_stagePrefab);
        if (stage == null) return null;

        stage.ResetData(_mobStageData);
        return stage;
    }
}

public class ChapterStageFactory : BaseFactory
{
    Dictionary<GameMode.Level, Dictionary<BaseStage.Name, StageCreater>> _stageCreaters;

    public ChapterStageFactory(Dictionary<GameMode.Level, ILevel> stagePrefab, Dictionary<GameMode.Level, ILevelData> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, Dictionary<BaseStage.Name, StageCreater>>();

        List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < levels.Count; i++)
        {
            _stageCreaters[levels[i]] = new Dictionary<BaseStage.Name, StageCreater>();
        }

        for (int i = 0; i < levels.Count; i++)
        {
            GameMode.Level level = levels[i];
            _stageCreaters[level][BaseStage.Name.StartStage] = new NormalStageCreater(stagePrefab[level].StartStage);
            _stageCreaters[level][BaseStage.Name.BonusStage] = new NormalStageCreater(stagePrefab[level].BonusStage);

            _stageCreaters[level][BaseStage.Name.BossStage] = new BossStageCreater(stagePrefab[level].BossStage, stageData[level].BossStageData);

            _stageCreaters[level][BaseStage.Name.MobStage1] = new MobStageCreater(stagePrefab[level].MobStages[0], stageData[level].MobStageDatas[0]);
            _stageCreaters[level][BaseStage.Name.MobStage2] = new MobStageCreater(stagePrefab[level].MobStages[1], stageData[level].MobStageDatas[1]);
            _stageCreaters[level][BaseStage.Name.MobStage3] = new MobStageCreater(stagePrefab[level].MobStages[2], stageData[level].MobStageDatas[2]);
            _stageCreaters[level][BaseStage.Name.MobStage4] = new MobStageCreater(stagePrefab[level].MobStages[3], stageData[level].MobStageDatas[3]);
            _stageCreaters[level][BaseStage.Name.MobStage5] = new MobStageCreater(stagePrefab[level].MobStages[4], stageData[level].MobStageDatas[4]);
        }

    }

    public override BaseStage Create(GameMode.Level chapter, BaseStage.Name name)
    {
        return _stageCreaters[chapter][name].Create();
    }
}