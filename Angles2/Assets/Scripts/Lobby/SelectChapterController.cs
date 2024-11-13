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

    Dictionary<DungeonMode.Chapter, ChapterInfo> _chapterInfos;
    public Action<DungeonMode.Chapter> OnSelectChapter;

    public void Activate(bool on) => _content.SetActive(on);

    public void ChangeChapter(DungeonMode.Chapter chapter)
    {
        _selectScrollUI.ScrollUsingChapter(chapter);
        OnChapterSelected(chapter);
    }

    void OnChooseChapter()
    {
        int index = _selectScrollUI.TargetIndex;
        if (_chapterInfos[(DungeonMode.Chapter)index]._nowLock == true) return; // 잠겨있다면 진행 X

        Activate(false);
        OnSelectChapter?.Invoke((DungeonMode.Chapter)index);
    }

    void OnChapterSelected(DungeonMode.Chapter chapter)
    {
        _selectChapterModel.Title = chapter.ToString();
        _selectChapterModel.Info = _chapterInfos[chapter]._content;
        _selectChapterModel.Level = _chapterInfos[chapter]._maxLevel;
    }

    public void Initialize(Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos, Dictionary<DungeonMode.Chapter, Sprite> chapterSprite, BaseFactory viewerFactory, Action<DungeonMode.Chapter> OnSelectChapter)
    {
        this.OnSelectChapter = OnSelectChapter;
        _selectChapterModel = new SelectChapterModel(_selectChapterViewer);

        // chapterInfos는 여기서 관리
        _chapterInfos = chapterInfos;
        _selectScrollUI.Initialize(chapterInfos, chapterSprite, viewerFactory, OnChapterSelected);
        _selectBtn.onClick.AddListener(() => { ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click); OnChooseChapter(); });
    }
}