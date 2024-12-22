using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class LevelSelectPage : MonoBehaviour
{
    [SerializeField] SelectLevelController _selectChapterController;

    [SerializeField] Button _selectChapterBtn; // �������� ���� ��ư
    [SerializeField] PlayLevelViewer _playChapterViewer;
    PlayLevelModel _playChapterModel;

    [SerializeField] Button _playChapterBtn;

    [SerializeField] Toggle _survivalToggle;
    [SerializeField] Toggle _chapterToggle;

    GameMode.Type _levelType;

    Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> _typeDatas;

    // �Ŵ� UI ���� �����ֱ�
    void SelectLevel(GameMode.Level level)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentLevel(_levelType, level);

        _playChapterModel.Title = _typeDatas[_levelType][level].LevelInfos.Title;
        _playChapterModel.LevelSprite = _typeDatas[_levelType][level].LevelSprite;
        _playChapterModel.LevelInfo = new Tuple<GameMode.Type, ISavableLevelInfo, ILevelInfo>
        (
            _typeDatas[_levelType][level].LevelInfos.Type,
            _typeDatas[_levelType][level].SavableLevelInfos,
            _typeDatas[_levelType][level].LevelInfos
        );
    }

    // ���� ��� Ÿ���� �ٲ� ���
    void OnChangeGameModeType()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������
        SelectLevel(saveData._selectedLevel[_levelType]);
    }

    public void Initialize(
        Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> typeDatas,
        BaseFactory viewerFactory)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������

        _typeDatas = typeDatas;
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
            typeDatas,
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
        ); // ���⿡ ���� ���� ��� �߰�
    }
}
