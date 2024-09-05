using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageModel
{
    BaseViewer _stageCountViewer;
    BaseViewer _stageResultViewer;

    public StageModel(BaseViewer stageCountViewer, BaseViewer stageResultViewer)
    {
        _stageCountViewer = stageCountViewer;
        _stageResultViewer = stageResultViewer;

        StageCount = 0;
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
    [SerializeField] BaseViewer _stageCountViewer;
    [SerializeField] BaseViewer _stageResultViewer;

    StageModel _stageModel;

    public void Initialize()
    {
        _stageModel = new StageModel(_stageCountViewer, _stageResultViewer);
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
