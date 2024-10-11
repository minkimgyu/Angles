using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] SelectChapterController _selectChapterController;

    [SerializeField] Button _selectChapterBtn; // 스테이지 선택 버튼
    [SerializeField] PlayChapterViewer _playChapterViewer;
    PlayChapterModel _playChapterModel;

    [SerializeField] Button _playChapterBtn;

    Dictionary<DungeonChapter, ChapterInfo> _chapterInfos;
    Dictionary<DungeonChapter, Sprite> _chapterSprite;

    void SelectChapter(DungeonChapter chapter)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentStage(chapter);

        _playChapterModel.Title = chapter.ToString();
        _playChapterModel.ChapterIcon = _chapterSprite[chapter];
        _playChapterModel.Progress = new Tuple<int, int>(_chapterInfos[chapter]._completeLevel, _chapterInfos[chapter]._maxLevel);
    }

    public void Initialize(
        DungeonChapter currentChapter,
        Dictionary<DungeonChapter, ChapterInfo> chapterInfos,
        Dictionary<DungeonChapter, Sprite> chapterSprite,
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

                SaveData saveData = ServiceLocater.ReturnSaveManager().ReturnSaveData();
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
