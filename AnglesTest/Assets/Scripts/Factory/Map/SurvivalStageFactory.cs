using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStageCreater : StageCreater
{
    SurvivalStageData _survivalStageData;

    public SurvivalStageCreater(BaseStage stagePrefab, SurvivalStageData survivalStageData) : base(stagePrefab)
    {
        _survivalStageData = survivalStageData;
    }

    public override BaseStage Create()
    {
        BaseStage stage = Object.Instantiate(_stagePrefab);
        if (stage == null) return null;

        stage.ResetData(_survivalStageData);
        return stage;
    }
}

public class SurvivalStageFactory : BaseFactory
{
    Dictionary<GameMode.Level, StageCreater> _stageCreaters;

    public SurvivalStageFactory(Dictionary<GameMode.Level, ILevel> stagePrefab, Dictionary<GameMode.Level, ILevelData> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, StageCreater>();

        List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Survival);
        for (int i = 0; i < levels.Count; i++)
        {
            _stageCreaters[levels[i]] = new SurvivalStageCreater(stagePrefab[levels[i]].SurvivalStageLevel, (SurvivalStageData)stageData[levels[i]]);
        }
    }

    public override BaseStage Create(GameMode.Level level)
    {
        return _stageCreaters[level].Create();
    }
}
