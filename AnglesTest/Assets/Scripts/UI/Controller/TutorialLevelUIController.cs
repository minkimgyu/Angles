using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelModel
{
    StageCountViewer _stageCountViewer;
    StageResultViewer _stageResultViewer;

    public TutorialLevelModel(StageCountViewer stageCountViewer, StageResultViewer stageResultViewer)
    {
        _stageCountViewer = stageCountViewer;
        _stageResultViewer = stageResultViewer;

        StageCount = 0;
        StageResultInfo = "";
        ShowStageResult = false;
    }

    int _stageCount = 0;
    public int StageCount
    {
        get => _stageCount;
        set
        {
            _stageCount = value;
            _stageCountViewer.UpdateStaegCount(_stageCount.ToString());
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

public class TutorialLevelUIController : MonoBehaviour
{
    [SerializeField] StageCountViewer _stageCountViewer;
    [SerializeField] StageResultViewer _stageResultViewer;
    TutorialLevelModel _levelModel;

    public void Initialize()
    {
        _levelModel = new TutorialLevelModel(_stageCountViewer, _stageResultViewer);
    }

    public void ChangeStageCount(int count)
    {
        _levelModel.StageCount = count;
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
