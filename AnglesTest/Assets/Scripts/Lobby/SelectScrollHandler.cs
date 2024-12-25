using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectScrollHandler : ScrollUI
{
    HorizontalLayoutGroup _horizontalLayoutGroup;
    float _selectedSize = 1.3f;
    Timer _scaleChangeTimer;
    BaseFactory _viewerFactory;

    public GameMode.Level CurrentLevel 
    { 
        get 
        { 
            return GameMode.GetLevel(_storedLevelType, _targetIndex); 
        } 
    }

    RectTransform _rectTransform;

    Action<GameMode.Level> OnLevelSelected;
    List<ChapterViewer> _spawnedChapterViewers;

    Dictionary<GameMode.Level, LevelData> _levelDatas;

    // Initialize를 2개 만들어서 사용하자
    public void Initialize(
        Dictionary<GameMode.Level, LevelData> levelData,
        BaseFactory viewerFactory,
        Action<GameMode.Level> OnLevelSelected)
    {
        int chapterRectHalfSize = (int)(350 * 1.3 / 2);
        int offset = (Screen.width / 2) - chapterRectHalfSize;

        _horizontalLayoutGroup = _contentTr.GetComponent<HorizontalLayoutGroup>();
        _horizontalLayoutGroup.padding.left = offset;
        _horizontalLayoutGroup.padding.right = offset;

        _levelDatas = levelData;

        this.OnLevelSelected = OnLevelSelected;
        _rectTransform = GetComponent<RectTransform>();

        _viewerFactory = viewerFactory;
        _scaleChangeTimer = new Timer();
        _spawnedChapterViewers = new List<ChapterViewer>();

        // 키면 생성, 끄면 다 부서준다.
        // 이런 방식으로 개발
    }

    GameMode.Type _storedLevelType;

    public void CreateChapterViewer(GameMode.Type levelType)
    {
        _storedLevelType = levelType;

        int chapterCount = _levelDatas.Count;
        SetUp(chapterCount);

        List<GameMode.Level> levels = GameMode.GetLevels(levelType);
        for (int i = 0; i < levels.Count; i++)
        {
            ChapterViewer viewer = (ChapterViewer)_viewerFactory.Create(BaseViewer.Name.ChapterSelectViewer);
            viewer.Initialize(_levelDatas[levels[i]].LevelSprite, _levelDatas[levels[i]].SavableLevelInfos.NowUnlock);
            viewer.transform.SetParent(_contentTr);

            _spawnedChapterViewers.Add(viewer);
        }

        ScaleTarget();
    }

    public void DestroyChapterViewer()
    {
        for (int i = 0; i < _spawnedChapterViewers.Count; i++)
        {
            Destroy(_spawnedChapterViewers[i].gameObject);
            _spawnedChapterViewers.Remove(_spawnedChapterViewers[i]);
            i--;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
       base.OnEndDrag(eventData);

        GameMode.Level level = GameMode.GetLevel(_storedLevelType, _targetIndex);
        OnLevelSelected?.Invoke(level);

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    public void ScrollUsingChapter(GameMode.Level level)
    {
        _currentPos = SetPos();

        _targetIndex = GameMode.GetLevelIndex(_storedLevelType, level);
        _targetPos = _points[_targetIndex];

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    void ScaleTarget(float ratio = 1)
    {
        for (int i = 0; i < _contentTr.childCount; i++)
        {
            if (i == _targetIndex)
            {
                Transform targetTr = _contentTr.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one * _selectedSize, ratio);
            }
            else
            {
                Transform targetTr = _contentTr.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one, ratio);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_scaleChangeTimer.CurrentState == Timer.State.Running)
        {
            ScaleTarget(_scaleChangeTimer.Ratio);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }
    }
}
