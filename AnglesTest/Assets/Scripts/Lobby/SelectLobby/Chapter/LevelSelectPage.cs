using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class LevelSelectPage : MonoBehaviour
{
    [SerializeField] SelectLevelController _selectChapterController;

    [SerializeField] Button _selectChapterBtn; // 스테이지 선택 버튼
    [SerializeField] PlayLevelViewer _playChapterViewer;
    PlayLevelModel _playChapterModel;

    [SerializeField] Button _playChapterBtn;

    [SerializeField] Toggle _survivalToggle;
    [SerializeField] Toggle _chapterToggle;

    GameMode.Type _levelType;

    Dictionary<GameMode.Level, LevelData> _levelDatas;

    // 매뉴 UI 정보 보여주기
    void SelectLevel(GameMode.Level level)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentLevel(_levelType, level);

        string title = ServiceLocater.ReturnLocalizationHandler().GetWord($"{level}Name");

        _playChapterModel.Title = title;
        _playChapterModel.LevelSprite = _levelDatas[level].LevelSprite;
        _playChapterModel.LevelInfo = new Tuple<GameMode.Type, ISavableLevelInfo, ILevelInfo>
        (
            _levelDatas[level].LevelInfos.Type,
            _levelDatas[level].SavableLevelInfos,
            _levelDatas[level].LevelInfos
        );
    }

    // 게임 모드 타입을 바꾼 경우
    void OnChangeGameModeType()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터
        SelectLevel(saveData._selectedLevel[_levelType]);
    }

    public void Initialize(
        Dictionary<GameMode.Level, LevelData> levelDatas,
        BaseFactory viewerFactory)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        _levelDatas = levelDatas;
        _playChapterModel = new PlayLevelModel(_playChapterViewer);

        _survivalToggle.onValueChanged.AddListener
        (
            (isOn) => 
            { 
                if (isOn)
                {
                    _levelType = GameMode.Type.Survival;
                    OnChangeGameModeType();
                }
            }
        );

        _chapterToggle.onValueChanged.AddListener
        (
            (isOn) => 
            { 
                if (isOn)
                {
                    _levelType = GameMode.Type.Chapter;
                    OnChangeGameModeType();
                }
            }
        );

        _levelType = GameMode.Type.Chapter;
        _survivalToggle.isOn = false;
        _chapterToggle.isOn = true;

        _selectChapterController.Initialize(
            levelDatas,
            viewerFactory,
            SelectLevel
        );

        _selectChapterBtn.onClick.AddListener(
            () => 
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);
                _selectChapterController.Activate(true, _levelType);

                SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
                _selectChapterController.ChangeChapter(_levelType, saveData._selectedLevel[_levelType]);
            }
        );

        _playChapterBtn.onClick.AddListener(
            () => 
            {
                ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);
                ISceneControllable controller = ServiceLocater.ReturnSceneController();

                ISceneControllable.SceneName name = 
                (ISceneControllable.SceneName)Enum.Parse(typeof(ISceneControllable.SceneName), $"{_levelType.ToString()}Scene");
                controller.ChangeScene(name);
            }
        ); // 여기에 게임 시작 기능 추가
    }
}
