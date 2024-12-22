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

public class SurvivalStageCreater : StageCreater
{
    MobStageData _mobStageData;

    public SurvivalStageCreater(BaseStage stagePrefab, MobStageData mobStageData) : base(stagePrefab)
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

    public ChapterStageFactory(Dictionary<GameMode.Level, Dictionary<BaseStage.Name, BaseStage>> stagePrefab, Dictionary<GameMode.Level, Dictionary<BaseStage.Name, IStageData>> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, Dictionary<BaseStage.Name, StageCreater>>();

        List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Chapter);
        for (int i = 0; i < levels.Count; i++)
        {
            _stageCreaters[levels[i]] = new Dictionary<BaseStage.Name, StageCreater>();
        }

        for (int i = 0; i < levels.Count; i++)
        {
            GameMode.Level chapter = levels[i];
            _stageCreaters[chapter][BaseStage.Name.StartStage] = new NormalStageCreater(stagePrefab[chapter][BaseStage.Name.StartStage]);
            _stageCreaters[chapter][BaseStage.Name.BonusStage] = new NormalStageCreater(stagePrefab[chapter][BaseStage.Name.BonusStage]);

            _stageCreaters[chapter][BaseStage.Name.BossStage] = new BossStageCreater(stagePrefab[chapter][BaseStage.Name.BossStage], (BossStageData)stageData[chapter][BaseStage.Name.BossStage]);

            _stageCreaters[chapter][BaseStage.Name.MobStage1] = new MobStageCreater(stagePrefab[chapter][BaseStage.Name.MobStage1], (MobStageData)stageData[chapter][BaseStage.Name.MobStage1]);
            _stageCreaters[chapter][BaseStage.Name.MobStage2] = new MobStageCreater(stagePrefab[chapter][BaseStage.Name.MobStage2], (MobStageData)stageData[chapter][BaseStage.Name.MobStage2]);
            _stageCreaters[chapter][BaseStage.Name.MobStage3] = new MobStageCreater(stagePrefab[chapter][BaseStage.Name.MobStage3], (MobStageData)stageData[chapter][BaseStage.Name.MobStage3]);
            _stageCreaters[chapter][BaseStage.Name.MobStage4] = new MobStageCreater(stagePrefab[chapter][BaseStage.Name.MobStage4], (MobStageData)stageData[chapter][BaseStage.Name.MobStage4]);
            _stageCreaters[chapter][BaseStage.Name.MobStage5] = new MobStageCreater(stagePrefab[chapter][BaseStage.Name.MobStage5], (MobStageData)stageData[chapter][BaseStage.Name.MobStage5]);
        }

    }

    public override BaseStage Create(GameMode.Level chapter, BaseStage.Name name)
    {
        return _stageCreaters[chapter][name].Create();
    }
}