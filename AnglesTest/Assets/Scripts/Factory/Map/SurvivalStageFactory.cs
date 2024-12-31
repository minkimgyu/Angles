using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStageCreater : StageCreater
{
    SurvivalStageData _survivalStageData;
    ILevel _survivalStage;

    public SurvivalStageCreater(SurvivalStage survivalStage, SurvivalStageData survivalStageData)
    {
        _survivalStage = survivalStage;
        _survivalStageData = survivalStageData;
    }

    public override ILevel Create()
    {
        ILevel stage = Object.Instantiate(_survivalStage as SurvivalStage);
        if (stage == null) return null;

        stage.SurvivalStageLevel.ResetData(_survivalStageData);
        return stage;
    }
}

public class SurvivalStageFactory : BaseFactory
{
    Dictionary<GameMode.Level, StageCreater> _stageCreaters;

    public SurvivalStageFactory(Dictionary<GameMode.Level, ILevel> stagePrefab, Dictionary<GameMode.Level, ILevelData> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, StageCreater>();

        try
        {
            List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Survival);
            for (int i = 0; i < levels.Count; i++)
            {
                _stageCreaters[levels[i]] = new SurvivalStageCreater(stagePrefab[levels[i]].SurvivalStageLevel, (SurvivalStageData)stageData[levels[i]]);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public override ILevel Create(GameMode.Level level)
    {
        return _stageCreaters[level].Create();
    }
}
