using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            _selectChapterViewer.ChangeLevel(_level);
        }
    }

    string _info;
    public string Info
    {
        get { return _info; }
        set
        {
            _info = value;
            _selectChapterViewer.ChangeInfo(_info);
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

    public void ChangeLevel(int level)
    {
        _levelTxt.text = $"스테이지 수 : {level}";
    }

    public void ChangeInfo(string info)
    {
        _infoTxt.text = info;
    }
}
