using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SelectLevelController : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] SelectScrollHandler _selectScrollHandler;
    [SerializeField] Button _selectBtn;

    [SerializeField] SelectChapterViewer _selectChapterViewer;
    SelectChapterModel _selectChapterModel;

    Dictionary<GameMode.Level, LevelData> _levelDatas;
    Action<GameMode.Level> OnSelectLevel;









    public void Activate(bool on, GameMode.Type levelType = default)
    {
        _content.SetActive(on);

        if(on) _selectScrollHandler.CreateChapterViewer(levelType);
        else _selectScrollHandler.DestroyChapterViewer();
    }

    GameMode.Type _storedLevelType;

    public void ChangeChapter(GameMode.Type type, GameMode.Level level)
    {
        _storedLevelType = type;
        _selectScrollHandler.ScrollUsingChapter((level));
        ChangeChapterModel(level);
    }

    /// <summary>
    /// ���� ���� �� �۵�
    /// </summary>
    void OnChooseLevel()
    {
        GameMode.Level level = _selectScrollHandler.CurrentLevel;
        if (_levelDatas[level].SavableLevelInfos.NowUnlock == false) return; // �رݵ��� �ʾҴٸ� ���� X

        Activate(false);
        OnSelectLevel?.Invoke(level);
    }

    void ChangeChapterModel(GameMode.Level level)
    {
        _selectChapterModel.Title = _levelDatas[level].LevelInfos.Title;
        _selectChapterModel.Description = _levelDatas[level].LevelInfos.Description;

        _selectChapterModel.LevelInfo = new Tuple<GameMode.Type, ILevelInfo>
        (
            _levelDatas[level].LevelInfos.Type,
            _levelDatas[level].LevelInfos
        );
    }

    public void Initialize(
        Dictionary<GameMode.Level, LevelData> typeDatas,
        BaseFactory viewerFactory,
        Action<GameMode.Level> OnSelectLevel)
    {
        _levelDatas = typeDatas;
        this.OnSelectLevel = OnSelectLevel;

        _selectChapterModel = new SelectChapterModel(_selectChapterViewer);
        _selectScrollHandler.Initialize
        (
            typeDatas,
            viewerFactory,
            ChangeChapterModel
        );

        _selectBtn.onClick.AddListener(() => { 
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click); 
            OnChooseLevel(); 
        });
    }
}