using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class SelectChapterModel
{
    SelectChapterViewer _selectChapterViewer;

    public SelectChapterModel(SelectChapterViewer selectChapterViewer)
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
            }
        }
    }
}

public class SelectChapterViewer : MonoBehaviour
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
        _levelTxt.text = $"스테이지 수 : {level}";
    }

    public void ChangeSurvivalStageProgress(int survivalTime)
    {
        _levelTxt.text = $"생존 시간 : {survivalTime / 60}:{(survivalTime % 60).ToString("D2")}";
    }

    public void ChangeInfo(string info)
    {
        _infoTxt.text = info;
    }
}
