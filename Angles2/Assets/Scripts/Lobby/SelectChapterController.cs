using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;

public struct ChapterInfo
{
    public string _content;
    public int _completeLevel;
    public int _maxLevel;

    public bool _nowLock; // 이전 스테이지를 클리어해야 해금된다.

    public ChapterInfo(string content, int completeLevel, int maxLevel, bool nowLock)
    {
        _content = content;
        _completeLevel = completeLevel;
        _maxLevel = maxLevel;
        _nowLock = nowLock;
    }
}

public class SelectChapterController : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] SelectScrollUI _selectScrollUI;
    [SerializeField] Button _selectBtn;

    [SerializeField] SelectChapterViewer _selectChapterViewer;
    SelectChapterModel _selectChapterModel;

    Dictionary<DungeonChapter, ChapterInfo> _chapterInfos;
    public Action<DungeonChapter> OnSelectChapter;

    public void Activate(bool on) => _content.SetActive(on);

    public void ChangeChapter(DungeonChapter chapter)
    {
        _selectScrollUI.ScrollUsingChapter(chapter);
        OnChapterSelected(chapter);
    }

    void OnChooseChapter()
    {
        int index = _selectScrollUI.TargetIndex;
        if (_chapterInfos[(DungeonChapter)index]._nowLock == true) return; // 잠겨있다면 진행 X

        Activate(false);
        OnSelectChapter?.Invoke((DungeonChapter)index);
    }

    void OnChapterSelected(DungeonChapter chapter)
    {
        _selectChapterModel.Title = chapter.ToString();
        _selectChapterModel.Info = _chapterInfos[chapter]._content;
        _selectChapterModel.Level = _chapterInfos[chapter]._maxLevel;
    }

    public void Initialize(Dictionary<DungeonChapter, ChapterInfo> chapterInfos, Dictionary<DungeonChapter, Sprite> chapterSprite, BaseFactory viewerFactory, Action<DungeonChapter> OnSelectChapter)
    {
        this.OnSelectChapter = OnSelectChapter;
        _selectChapterModel = new SelectChapterModel(_selectChapterViewer);

        // chapterInfos는 여기서 관리
        _chapterInfos = chapterInfos;
        _selectScrollUI.Initialize(chapterInfos, chapterSprite, viewerFactory, OnChapterSelected);
        _selectBtn.onClick.AddListener(() => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click); OnChooseChapter(); });
    }
}