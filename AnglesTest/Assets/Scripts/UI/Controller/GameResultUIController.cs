using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameResultModel
{
    GameResultViewer _gameEndViewer;

    public GameResultModel(GameResultViewer gameEndViewer)
    {
        _gameEndViewer = gameEndViewer;
    }

    public void ResetData(bool turnOn)
    {
        _gameEndViewer.TurnOnViewer(turnOn);
    }

    public void ResetData(float recordTime, int coinCount, bool turnOn)
    {
        _gameEndViewer.TurnOnViewer(turnOn);
        _gameEndViewer.ChangeRecord(recordTime);
        _gameEndViewer.ChangeCoin(coinCount);

        _gameEndViewer.FadeInOutTabTxt();
    }

    float _recordTime;
    int _coinCount;
}

public class GameResultUIController : MonoBehaviour
{
    [SerializeField] GameResultViewer _gameClearViewer;
    [SerializeField] GameResultViewer _gameFailViewer;

    GameResultModel _gameClearModel;
    GameResultModel _gameFailModel;

    public void Initialize(System.Action OnReturnToMenuRequested)
    {
        _gameClearViewer.Initialize(OnReturnToMenuRequested);
        _gameClearModel = new GameResultModel(_gameClearViewer);

        _gameFailViewer.Initialize(OnReturnToMenuRequested);
        _gameFailModel = new GameResultModel(_gameFailViewer);

        _gameClearModel.ResetData(false);
        _gameFailModel.ResetData(false);
    }

    public void OnClearRequested(float recordTime, int coinCount)
    {
        _gameClearModel.ResetData(recordTime, coinCount, true);
    }

    public void OnFailRequested(float recordTime, int coinCount)
    {
        _gameFailModel.ResetData(recordTime, coinCount, true);
    }
}
