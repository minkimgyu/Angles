using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelectPage : MonoBehaviour
{
    [SerializeField] SelectChapterController _selectChapterController;

    [SerializeField] Button _selectChapterBtn; // 스테이지 선택 버튼
    [SerializeField] PlayChapterViewer _playChapterViewer;
    PlayChapterModel _playChapterModel;

    [SerializeField] Button _playChapterBtn;

    Dictionary<DungeonMode.Chapter, ChapterInfo> _chapterInfos;
    Dictionary<DungeonMode.Chapter, Sprite> _chapterSprite;

    void SelectChapter(DungeonMode.Chapter chapter)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentStage(chapter);

        _playChapterModel.Title = _chapterInfos[chapter]._name;
        _playChapterModel.ChapterIcon = _chapterSprite[chapter];
        _playChapterModel.Progress = new Tuple<int, int>(_chapterInfos[chapter]._completeLevel, _chapterInfos[chapter]._maxLevel);
    }

    public void Initialize(
        DungeonMode.Chapter currentChapter,
        Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos,
        Dictionary<DungeonMode.Chapter, Sprite> chapterSprite,
        BaseFactory viewerFactory)
    {
        _chapterInfos = chapterInfos;
        _chapterSprite = chapterSprite;

        _playChapterModel = new PlayChapterModel(_playChapterViewer);
        SelectChapter(currentChapter); // 최초 초기화

        _selectChapterController.Initialize(
            chapterInfos,
            chapterSprite,
            viewerFactory,
            SelectChapter
        );

        _selectChapterBtn.onClick.AddListener(
            () => 
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);
                _selectChapterController.Activate(true);

                SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
                _selectChapterController.ChangeChapter(saveData._chapter);
            }
        );

        _playChapterBtn.onClick.AddListener(
            () => 
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);
                ISceneControllable controller = ServiceLocater.ReturnSceneController();
                controller.ChangeScene("PlayScene");
            }
        ); // 여기에 게임 시작 기능 추가
    }
}
