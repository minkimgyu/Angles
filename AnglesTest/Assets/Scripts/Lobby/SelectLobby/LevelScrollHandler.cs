using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelScrollHandler : ScrollUI
{
    HorizontalLayoutGroup _horizontalLayoutGroup;
    const float _selectedSize = 1.3f;
    Timer _scaleChangeTimer;
    BaseFactory _viewerFactory;

    public GameMode.Level CurrentLevel
    {
        get
        {
            return GameMode.GetLevel(_storedModeType, _targetIndex);
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
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        RectTransform rectTransform = parentCanvas.GetComponent<RectTransform>();

        int chapterRectHalfSize = (int)(350 * _selectedSize / 2);
        int offset = ((int)rectTransform.rect.width / 2) - chapterRectHalfSize;

        _horizontalLayoutGroup = _content.GetComponent<HorizontalLayoutGroup>();
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

    GameMode.Type _storedModeType;

    public void CreateChapterViewer(GameMode.Type modeType)
    {
        _storedModeType = modeType;

        int chapterCount = GameMode.GetLevelCount(modeType);
        SetUp(chapterCount);

        List<GameMode.Level> levels = GameMode.GetLevels(modeType);
        for (int i = 0; i < levels.Count; i++)
        {
            ChapterViewer viewer = (ChapterViewer)_viewerFactory.Create(BaseViewer.Name.ChapterSelectViewer);
            viewer.Initialize(_levelDatas[levels[i]].LevelSprite, _levelDatas[levels[i]].SavableLevelInfos.NowUnlock);
            viewer.transform.SetParent(_content);

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

        GameMode.Level level = GameMode.GetLevel(_storedModeType, _targetIndex);
        OnLevelSelected?.Invoke(level);

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    public void ScrollToLevel(GameMode.Level level)
    {
        _currentPos = GetPos();

        _targetIndex = GameMode.GetLevelIndex(_storedModeType, level);
        _targetPos = _points[_targetIndex];

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    void ScaleTarget(float ratio = 1)
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            if (i == _targetIndex)
            {
                Transform targetTr = _content.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one * _selectedSize, ratio);
            }
            else
            {
                Transform targetTr = _content.GetChild(i);
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
