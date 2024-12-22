using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayLevelModel
{
    PlayLevelViewer _playChapterViewer;

    public PlayLevelModel(PlayLevelViewer selectChapterViewer)
    {
        _playChapterViewer = selectChapterViewer;
    }

    string _title;
    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            _playChapterViewer.ChangeTitle(_title);
        }
    }

    Sprite _levelSprite;
    public Sprite LevelSprite
    {
        get { return _levelSprite; }
        set
        {
            _levelSprite = value;
            _playChapterViewer.ChangeChapterIcon(_levelSprite);
        }
    }

    Tuple<GameMode.Type, ISavableLevelInfo, ILevelInfo> _levelInfo;
    public Tuple<GameMode.Type, ISavableLevelInfo, ILevelInfo> LevelInfo
    {
        get { return _levelInfo; }
        set
        {
            _levelInfo = value;
            switch (_levelInfo.Item1)
            {
                case GameMode.Type.Chapter:
                    _playChapterViewer.ChangeChapterStageProgress(_levelInfo.Item2.CompleteLevel, _levelInfo.Item3.MaxLevel);
                    break;
                case GameMode.Type.Survival:
                    _playChapterViewer.ChangeSurvivalStageProgress(_levelInfo.Item2.CompleteDuration, _levelInfo.Item3.TotalDuration);
                    break;
            }
        }
    }
}

public class PlayLevelViewer : MonoBehaviour
{
    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] Image _chapterImg;
    [SerializeField] TMP_Text _progressTxt;

    public void ChangeTitle(string title)
    {
        _titleTxt.text = title;
    }

    public void ChangeChapterIcon(Sprite icon)
    {
        _chapterImg.sprite = icon;
    }

    public void ChangeChapterStageProgress(int currentLevel, int maxLevel)
    {
        _progressTxt.text = $"진행 : {currentLevel} / {maxLevel}";
    }

    public void ChangeSurvivalStageProgress(int survivalTime, int totalTime)
    {
        _progressTxt.text = $"진행 : {survivalTime / 60}:{(survivalTime % 60).ToString("D2")} / {totalTime / 60}:{(totalTime % 60).ToString("D2")}";
    }
}
