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

    [SerializeField] GameObject _toggleParent;
    [SerializeField] Toggle _survivalToggle;
    [SerializeField] Toggle _chapterToggle;

    GameMode.Type _levelType;

    Dictionary<GameMode.Level, LevelData> _levelDatas;

    // 레벨을 바꾼 경우
    void SelectLevel(GameMode.Level level)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentLevel(level); // 선택된 레벨로 변경
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
        saveable.ChangeType(_levelType);

        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터
        SelectLevel(saveData._selectedLevel[_levelType]); // 현재 선택된 레벨 변경
    }

    public void Initialize(
        Dictionary<GameMode.Level, LevelData> levelDatas,
        BaseFactory viewerFactory)
    {
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

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        // 기존 플레이 데이터가 있다면
        if(saveData.HavePlayData() == true)
        {
            _toggleParent.SetActive(true); // 토글 켜주기
            _levelType = saveData._selectedType; // 세이브된 게임 타입 적용
            SelectLevel(saveData._selectedLevel[_levelType]); // 게임 타입 선택
        }
        else // 기존 플레이 데이터가 없다면
        {
            _toggleParent.SetActive(false);// 토글 꺼주기
            _levelType = GameMode.Type.Tutorial; // 튜토리얼 타입 적용
            SelectLevel(saveData._selectedLevel[_levelType]); // 게임 타입 선택
        }

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
