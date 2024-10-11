using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageModel
{
    StageCountViewer _stageCountViewer;
    StageResultViewer _stageResultViewer;
    BossHpViewer _bossHpViewer;

    public StageModel(StageCountViewer stageCountViewer, BossHpViewer bossHpViewer, StageResultViewer stageResultViewer)
    {
        _stageCountViewer = stageCountViewer;
        _stageResultViewer = stageResultViewer;
        _bossHpViewer = bossHpViewer;

        StageCount = 0;
    }

    bool _showStageCountViewer = false;
    public bool ShowStageCountViewer
    {
        get => _showStageCountViewer;
        set
        {
            _showBossHPViewer = value;
            _stageCountViewer.TurnOnViewer(_showStageCountViewer);
        }
    }


    bool _showBossHPViewer = false;
    public bool ShowBossHPViewer
    {
        get => _showBossHPViewer;
        set
        {
            _showBossHPViewer = value;
            _bossHpViewer.TurnOnViewer(_showBossHPViewer);
        }
    }

    float _bossHpRatio = 1f;
    public float BossHpRatio
    {
        get => _bossHpRatio;
        set
        {
            _bossHpRatio = value;
            _bossHpViewer.UpdateViewer(_bossHpRatio);
        }
    }

    int _stageCount = 0;
    public int StageCount
    {
        get => _stageCount;
        set
        {
            _stageCount = value;
            _stageCountViewer.UpdateViewer(_stageCount);
        }
    }

    string _stageResultInfo = "";
    public string StageResultInfo
    {
        get => _stageResultInfo;
        set
        {
            _stageResultInfo = value;
            _stageResultViewer.UpdateViewer(_stageResultInfo);
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

public class StageUIController : MonoBehaviour
{
    [SerializeField] StageCountViewer _stageCountViewer;
    [SerializeField] StageResultViewer _stageResultViewer;
    [SerializeField] BossHpViewer _bossHpViewer;
    StageModel _stageModel;

    public void Initialize()
    {
        _stageModel = new StageModel(_stageCountViewer, _bossHpViewer, _stageResultViewer);
        ShowBossHpViewer(false);
        ShowStageResult(true);
    }

    public void ShowStageCountViewer(bool nowShow)
    {
        _stageModel.ShowStageCountViewer = nowShow;
    }

    public void ShowBossHpViewer(bool nowShow)
    {
        _stageModel.ShowBossHPViewer = nowShow;
    }

    public void ChangeBossHpRatio(float ratio)
    {
        _stageModel.BossHpRatio = ratio;
    }

    public void AddStageCount(int count)
    {
        _stageModel.StageCount += count;
    }

    public void ShowStageResult(bool nowShow)
    {
        _stageModel.ShowStageResult = nowShow;
    }

    public void ChangeStageResultInfo(string info)
    {
        _stageModel.StageResultInfo = info;
    }
}
