using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayChapterModel
{
    PlayChapterViewer _playChapterViewer;

    public PlayChapterModel(PlayChapterViewer selectChapterViewer)
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

    Sprite _chapterIcon;
    public Sprite ChapterIcon
    {
        get { return _chapterIcon; }
        set
        {
            _chapterIcon = value;
            _playChapterViewer.ChangeChapterIcon(_chapterIcon);
        }
    }

    Tuple<int, int> _progress;
    public Tuple<int, int> Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;
            _playChapterViewer.ChangeLevel(_progress.Item1, _progress.Item2);
        }
    }
}

public class PlayChapterViewer : MonoBehaviour
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

    public void ChangeLevel(int currentLevel, int maxLevel)
    {
        _progressTxt.text = $"ม๘วเ : {currentLevel} / {maxLevel}";
    }
}
