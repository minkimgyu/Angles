using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStageCreater : StageCreater
{
    TutorialStageData _tutorialStageData;
    ILevel _tutorialStage;

    public TutorialStageCreater(TutorialStage tutorialStage, TutorialStageData survivalStageData)
    {
        _tutorialStage = tutorialStage;
        _tutorialStageData = survivalStageData;
    }

    public override ILevel Create()
    {
        ILevel stage = Object.Instantiate(_tutorialStage as TutorialStage);
        if (stage == null) return null;

        stage.TutorialStageLevel.ResetData(_tutorialStageData);
        return stage;
    }
}

public class TutorialStageFactory : BaseFactory
{
    Dictionary<GameMode.Level, StageCreater> _stageCreaters;

    public TutorialStageFactory(Dictionary<GameMode.Level, ILevel> stagePrefab, Dictionary<GameMode.Level, ILevelData> stageData)
    {
        _stageCreaters = new Dictionary<GameMode.Level, StageCreater>();

        List<GameMode.Level> levels = GameMode.GetLevels(GameMode.Type.Tutorial);
        for (int i = 0; i < levels.Count; i++)
        {
            _stageCreaters[levels[i]] = new TutorialStageCreater(stagePrefab[levels[i]].TutorialStageLevel, (TutorialStageData)stageData[levels[i]]);
        }
    }

    public override ILevel Create(GameMode.Level level)
    {
        return _stageCreaters[level].Create();
    }
}