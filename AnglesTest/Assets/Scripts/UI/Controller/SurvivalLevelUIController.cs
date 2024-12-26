using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SurvivalLevelModel
{
    StageCountViewer _stageCountViewer;
    CoinGaugeViewer _coinGaugeViewer;
    StageResultViewer _stageResultViewer;

    public SurvivalLevelModel(StageCountViewer stageCountViewer, CoinGaugeViewer coinGaugeViewer, StageResultViewer stageResultViewer)
    {
        _stageCountViewer = stageCountViewer;
        _coinGaugeViewer = coinGaugeViewer;
        _stageResultViewer = stageResultViewer;

        PassedTime = 0;
        CoinLevel = 0;
        NeedCoin = new Tuple<int, int>(0, 0);
        CoingaugeRatio = 0;
    }

    bool _showStageCountViewer = false;
    public bool ShowStageCountViewer
    {
        get => _showStageCountViewer;
        set
        {
            _showStageCountViewer = value;
            _stageCountViewer.TurnOnViewer(_showStageCountViewer);
        }
    }

    Tuple<int, int> _needCoin = new Tuple<int, int>(0, 0);
    public Tuple<int, int> NeedCoin
    {
        get => _needCoin;
        set
        {
            _needCoin = value;
            _coinGaugeViewer.UpdateNeedCoin(_needCoin);
        }
    }

    int _coinLevel = 0;
    public int CoinLevel
    {
        get => _coinLevel;
        set
        {
            _coinLevel = value;
            _coinGaugeViewer.UpdateLevel(_coinLevel);
        }
    }

    float _coingaugeRatio = 0;
    public float CoingaugeRatio
    {
        get => _coingaugeRatio;
        set
        {
            _coingaugeRatio = value;
            _coinGaugeViewer.UpdateRatio(_coingaugeRatio);
        }
    }

    int _passedTime = 0;
    public int PassedTime
    {
        get => _passedTime;
        set
        {
            _passedTime = value;

            string time = $"{_passedTime / 60}:{(_passedTime % 60).ToString("D2")}";
            _stageCountViewer.UpdateStaegCount(time);
        }
    }

    string _stageResultInfo = "";
    public string StageResultInfo
    {
        get => _stageResultInfo;
        set
        {
            _stageResultInfo = value;
            _stageResultViewer.UpdateResultInfo(_stageResultInfo);
        }
    }

    bool _showStageResult = false;
    public bool ShowStageResult
    {
        get => _showStageResult;
        set
        {
            _showStageResult = value;
            _stageResultViewer.TurnOnViewer(_showStageResult);
        }
    }
}

public class SurvivalLevelUIController : MonoBehaviour
{
    [SerializeField] StageCountViewer _stageCountViewer;
    [SerializeField] StageResultViewer _stageResultViewer;
    [SerializeField] CoinGaugeViewer _coinGaugeViewer;
    SurvivalLevelModel _levelModel;

    public void Initialize()
    {
        _levelModel = new SurvivalLevelModel(_stageCountViewer, _coinGaugeViewer, _stageResultViewer);
        ShowStageResult(false);
    }

    public void ShowStageCountViewer(bool nowShow)
    {
        _levelModel.ShowStageCountViewer = nowShow;
    }

    public void ChangePassedTime(int passedTime)
    {
        _levelModel.PassedTime = passedTime;
    }

    public void ChangeCoinLevel(int level)
    {
        _levelModel.CoinLevel = level;
    }

    public void ChangeNeedCoin(int currentCoin, int needCoin)
    {
        _levelModel.NeedCoin = new Tuple<int, int>(currentCoin, needCoin);
    }

    public void ChangeCoinGauge(float ratio)
    {
        _levelModel.CoingaugeRatio = ratio;
    }

    public void ShowStageResult(bool nowShow)
    {
        _levelModel.ShowStageResult = nowShow;
    }

    public void ChangeStageResultInfo(string info)
    {
        _levelModel.StageResultInfo = info;
    }
}
