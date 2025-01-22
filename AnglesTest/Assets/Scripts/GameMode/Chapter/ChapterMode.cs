using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class ChapterMode : DungeonMode
{
    ChapterStageController _stageController;

    // Start is called before the first frame update
    void Start()
    {
        Initialize(GameMode.Type.Chapter);
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount);

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        int stageCount = _stageController.ReturnCurrentStageCount();
        ServiceLocater.ReturnSaveManager().ChangeLevelProgress(data._selectedLevel[Type.Chapter], stageCount);
    }

    protected override void InitializeLevel(AddressableHandler addressableHandler, InGameFactory inGameFactory, Level level)
    {
        _stageController = GetComponent<ChapterStageController>();

        ILevelInfo levelInfo = addressableHandler.Database.LevelDatas[level];
        _stageController.Initialize(levelInfo.MaxLevel, addressableHandler, inGameFactory);
        _stageController.CreateRandomStage(level);
    }

    protected override void InitializeGameStageManager()
    {
        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState);
    }

    protected override BaseFactory ReturnStageFactory(AddressableHandler addressableHandler)
    {
        return new ChapterStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
    }
}
