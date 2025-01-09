using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalMode : DungeonMode
{
    [SerializeField] ArrowPointerController _arrowPointerController;
    [SerializeField] SurvivalLevelUIController _levelUIController;

    ILevel _level;

    List<int> _coinGaugeData;
    int _maxCoinLevel = 0;
    int _currentCoinLevel = 0;
    int _totalCoinCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize(GameMode.Type.Survival);
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        int passedTime = (int)_stopwatchTimer.Duration;
        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        ServiceLocater.ReturnSaveManager().ChangeLevelDuration(data._selectedLevel[Type.Survival], passedTime);
    }

    protected override void Update()
    {
        base.Update();

        float passedTime = _stopwatchTimer.Duration;
        _levelUIController.ChangePassedTime((int)passedTime);

        _level.SurvivalStageLevel.Spawn(passedTime);
    }

    protected override void InitializeLevel(AddressableHandler addressableHandler, InGameFactory inGameFactory, Level level)
    {
        _coinGaugeData = new List<int>(addressableHandler.Database.CoingaugeData);
        _maxCoinLevel = _coinGaugeData.Count - 1; // ÃÖ´ë 9

        _arrowPointerController.Initialize(inGameFactory);

        _levelUIController.Initialize();
        _levelUIController.ChangeNeedCoin(_totalCoinCount, _coinGaugeData[_currentCoinLevel]);


        ILevelInfo levelInfo = addressableHandler.Database.LevelDatas[level];

        _level = inGameFactory.GetFactory(InGameFactory.Type.Level).Create(level);
        _level.SurvivalStageLevel.transform.position = Vector2.zero;
        _level.SurvivalStageLevel.Initialize(this, addressableHandler, inGameFactory, _levelUIController, _arrowPointerController);
    }

    protected override void InitializeGameStageManager()
    {
        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState, (changeCount) =>
        {
            _totalCoinCount += changeCount;
            if (_currentCoinLevel < _maxCoinLevel && _totalCoinCount >= _coinGaugeData[_currentCoinLevel])
            {
                _currentCoinLevel++;
                EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateReusableCard, FindObjectOfType<Player>().GetCaster(), 3, 3);
            }

            _levelUIController.ChangeCoinLevel(_currentCoinLevel);
            _levelUIController.ChangeNeedCoin(_totalCoinCount, _coinGaugeData[_currentCoinLevel]);

            int decreaseAmount = 0;
            if (_currentCoinLevel == 0)
            {
                decreaseAmount = 0;
            }
            else
            {
                decreaseAmount = _coinGaugeData[_currentCoinLevel - 1];
            }


            float ratio = (float)(_totalCoinCount - decreaseAmount) / (_coinGaugeData[_currentCoinLevel] - decreaseAmount);
            _levelUIController.ChangeCoinGauge(ratio);
        });
    }

    protected override BaseFactory ReturnStageFactory(AddressableHandler addressableHandler)
    {
        return new SurvivalStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
    }
}
