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
    Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, StageCreater>> _stageCreaters;

    public ChapterStageFactory(Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, BaseStage>> stagePrefab, Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, IStageData>> stageData)
    {
        _stageCreaters = new Dictionary<DungeonMode.Chapter, Dictionary<BaseStage.Name, StageCreater>>();
        foreach (DungeonMode.Chapter chapter in Enum.GetValues(typeof(DungeonMode.Chapter)))
        {
            _stageCreaters[chapter] = new Dictionary<BaseStage.Name, StageCreater>();
        }

        int count = System.Enum.GetValues(typeof(DungeonMode.Chapter)).Length;
        for (int i = 0; i < count; i++)
        {
            DungeonMode.Chapter chapter = (DungeonMode.Chapter)i;

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

    public override BaseStage Create(DungeonMode.Chapter chapter, BaseStage.Name name)
    {
        return _stageCreaters[chapter][name].Create();
    }
}