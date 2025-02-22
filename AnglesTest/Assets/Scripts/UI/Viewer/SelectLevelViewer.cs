using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SelectLevelModel
{
    SelectLevelViewer _selectChapterViewer;

    public SelectLevelModel(SelectLevelViewer selectChapterViewer)
    {
        _selectChapterViewer = selectChapterViewer;
    }

    string _title;
    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            _selectChapterViewer.ChangeTitle(_title);
        }
    }

    string _description;
    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            _selectChapterViewer.ChangeInfo(_description);
        }
    }

    Tuple<GameMode.Type, ILevelInfo> _levelInfo;
    public Tuple<GameMode.Type, ILevelInfo> LevelInfo
    {
        get { return _levelInfo; }
        set
        {
            _levelInfo = value;
            switch (_levelInfo.Item1)
            {
                case GameMode.Type.Chapter:
                    _selectChapterViewer.ChangeChapterStageProgress(_levelInfo.Item2.MaxLevel);
                    break;
                case GameMode.Type.Survival:
                    _selectChapterViewer.ChangeSurvivalStageProgress(_levelInfo.Item2.TotalDuration);
                    break;
                case GameMode.Type.Tutorial:
                    _selectChapterViewer.ChangeChapterStageProgress(_levelInfo.Item2.MaxLevel);
                    break;
            }
        }
    }
}

public class SelectLevelViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _levelTxt;
    [SerializeField] TMP_Text _infoTxt;

    public void ChangeTitle(string title)
    {
        _titleTxt.text = title;
    }

    public void ChangeChapterStageProgress(int level)
    {
        string stageCount = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.StageCount);
        _levelTxt.text = $"{stageCount} : {level}";
    }

    public void ChangeSurvivalStageProgress(int survivalTime)
    {
        string surviveTime = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.SurvivalTime);
        _levelTxt.text = $"{surviveTime} : {survivalTime / 60}:{(survivalTime % 60).ToString("D2")}";
    }

    public void ChangeInfo(string info)
    {
        _infoTxt.text = info;
    }
}
