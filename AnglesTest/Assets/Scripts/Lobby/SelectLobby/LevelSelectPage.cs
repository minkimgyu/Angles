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

    [SerializeField] GameObject _toggleParent;
    [SerializeField] Toggle _survivalToggle;
    [SerializeField] Toggle _chapterToggle;

    GameMode.Type _levelType;

    Dictionary<GameMode.Level, LevelData> _levelDatas;

    // ������ �ٲ� ���
    void SelectLevel(GameMode.Level level)
    {
        ServiceLocater.ReturnSaveManager().ChangeCurrentLevel(level); // ���õ� ������ ����
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

    // ���� ��� Ÿ���� �ٲ� ���
    void OnChangeGameModeType()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        saveable.ChangeType(_levelType);

        SaveData saveData = saveable.GetSaveData(); // ����� ������
        SelectLevel(saveData._selectedLevel[_levelType]); // ���� ���õ� ���� ����
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
        SaveData saveData = saveable.GetSaveData(); // ����� ������

        // ���� �÷��� �����Ͱ� �ִٸ�
        if(saveData.HavePlayData() == true)
        {
            _toggleParent.SetActive(true); // ��� ���ֱ�
            _levelType = saveData._selectedType; // ���̺�� ���� Ÿ�� ����
            SelectLevel(saveData._selectedLevel[_levelType]); // ���� Ÿ�� ����
        }
        else // ���� �÷��� �����Ͱ� ���ٸ�
        {
            _toggleParent.SetActive(false);// ��� ���ֱ�
            _levelType = GameMode.Type.Tutorial; // Ʃ�丮�� Ÿ�� ����
            SelectLevel(saveData._selectedLevel[_levelType]); // ���� Ÿ�� ����
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
        ); // ���⿡ ���� ���� ��� �߰�
    }
}
