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

    Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> _typeDatas;
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
        ChangeChapterModel(type, level);
    }

    /// <summary>
    /// 레벨 선택 시 작동
    /// </summary>
    void OnChooseLevel()
    {
        GameMode.Level level = _selectScrollHandler.CurrentLevel;
        if (_typeDatas[_storedLevelType][level].SavableLevelInfos.NowUnlock == false) return; // 해금되지 않았다면 진행 X

        Activate(false);
        OnSelectLevel?.Invoke(level);
    }

    void ChangeChapterModel(GameMode.Type type, GameMode.Level level)
    {
        _selectChapterModel.Title = _typeDatas[type][level].LevelInfos.Title;
        _selectChapterModel.Description = _typeDatas[type][level].LevelInfos.Description;

        _selectChapterModel.LevelInfo = new Tuple<GameMode.Type, ILevelInfo>
        (
            _typeDatas[type][level].LevelInfos.Type,
            _typeDatas[type][level].LevelInfos
        );
    }

    public void Initialize(
        Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> typeDatas,
        BaseFactory viewerFactory,
        Action<GameMode.Level> OnSelectLevel)
    {
        _typeDatas = typeDatas;
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