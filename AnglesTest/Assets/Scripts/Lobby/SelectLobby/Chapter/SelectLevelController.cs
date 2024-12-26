using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class SelectLevelController : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] Button _selectBtn;

    [SerializeField] LevelScrollHandler _levelScrollHandler;
    [SerializeField] SelectLevelViewer _selectChapterViewer;
    SelectLevelModel _selectChapterModel;

    Dictionary<GameMode.Level, LevelData> _levelDatas;
    Action<GameMode.Level> OnSelectLevel;

    public void Activate(bool on, GameMode.Type levelType = default)
    {
        _content.SetActive(on);

        if(on) _levelScrollHandler.CreateChapterViewer(levelType);
        else _levelScrollHandler.DestroyChapterViewer();
    }

    GameMode.Type _storedModeType;

    public void ChangeChapter(GameMode.Type type, GameMode.Level level)
    {
        _storedModeType = type;
        _levelScrollHandler.ScrollToLevel((level));
        ChangeChapterModel(level);
    }

    /// <summary>
    /// 레벨 선택 시 작동
    /// </summary>
    void ChooseLevel()
    {
        GameMode.Level level = _levelScrollHandler.CurrentLevel;
        if (_levelDatas[level].SavableLevelInfos.NowUnlock == false) return; // 해금되지 않았다면 진행 X

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
        Dictionary<GameMode.Level, LevelData> levelDatas,
        BaseFactory viewerFactory,
        Action<GameMode.Level> OnSelectLevel)
    {
        _levelDatas = levelDatas;
        this.OnSelectLevel = OnSelectLevel;

        _selectChapterModel = new SelectLevelModel(_selectChapterViewer);

        _levelScrollHandler.Initialize
       (
           _levelDatas,
           viewerFactory,
           ChangeChapterModel
       );

        _selectBtn.onClick.AddListener(() => { 
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click); 
            ChooseLevel(); 
        });
    }
}