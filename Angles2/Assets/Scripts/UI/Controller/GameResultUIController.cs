using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameResultModel
{
    BaseViewer _gameEndViewer;

    public GameResultModel(BaseViewer gameEndViewer)
    {
        _gameEndViewer = gameEndViewer;
    }

    public void ResetData(float backgroundFadeRatio, float backgroundFadeDuration, string endInfo, Color labelColor, Color labelTxtColor)
    {
        _backgroundFadeDuration = backgroundFadeDuration;
        _backgroundFadeRatio = backgroundFadeRatio;
        _endInfo = endInfo;
        _labelColor = labelColor;
        _labelTxtColor = labelTxtColor;
        _gameEndViewer.TurnOnViewer(true, _backgroundFadeRatio, _backgroundFadeDuration, _endInfo, _labelColor, _labelTxtColor);
    }

    float _backgroundFadeRatio;
    float _backgroundFadeDuration;
    string _endInfo;
    Color _labelColor;
    Color _labelTxtColor;
}

public class GameResultUIController : MonoBehaviour
{
    [SerializeField] GameResultViewer _gameResultViewer;
    GameResultModel _gameResultModel;

    public void Initialize(System.Action OnReturnToMenuRequested)
    {
        _gameResultViewer.Initialize(OnReturnToMenuRequested);
        _gameResultModel = new GameResultModel(_gameResultViewer);
    }

    public void OnClearRequested()
    {
        _gameResultViewer.TurnOnViewer(true, 0.985f, 1.5f, "Game Clear", Color.white, Color.black);
    }

    public void OnFailRequested()
    {
        _gameResultViewer.TurnOnViewer(true, 0.985f, 1.5f, "Game Over", Color.black, Color.white);
    }
}
