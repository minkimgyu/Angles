using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectScrollUI : ScrollUI
{
    [SerializeField] HorizontalLayoutGroup _horizontalLayoutGroup;
    float _selectedSize = 1.3f;
    Timer _scaleChangeTimer;
    BaseFactory _viewerFactory;

    public int TargetIndex { get { return _targetIndex; } }

    RectTransform _rectTransform;

    Action<DungeonMode.Chapter> OnChapterSelected;

    public void Initialize(Dictionary<DungeonMode.Chapter, ChapterInfo> chapterInfos, Dictionary<DungeonMode.Chapter, Sprite> chapterSprite, BaseFactory viewerFactory, Action<DungeonMode.Chapter> OnChapterSelected)
    {
        int chapterRectHalfSize = (int)(350 * 1.3 / 2);
        int offset = (Screen.width / 2) - chapterRectHalfSize;

        _horizontalLayoutGroup.padding.left = offset;
        _horizontalLayoutGroup.padding.right = offset;



        this.OnChapterSelected = OnChapterSelected;
        _rectTransform = GetComponent<RectTransform>();

        int chapterCount = Enum.GetValues(typeof(DungeonMode.Chapter)).Length;
        SetUp(chapterCount);

        _viewerFactory = viewerFactory;
        _scaleChangeTimer = new Timer();

        for (int i = 0; i < chapterCount; i++)
        {
            BaseViewer viewer = _viewerFactory.Create(BaseViewer.Name.ChapterSelectViewer);
            DungeonMode.Chapter chapter = (DungeonMode.Chapter)i;

            viewer.Initialize(chapterSprite[chapter], chapterInfos[chapter]._nowLock);
            viewer.transform.SetParent(contentTr);
        }

        ScaleTarget();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
       base.OnEndDrag(eventData);


        OnChapterSelected?.Invoke((DungeonMode.Chapter)_targetIndex);

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    public void ScrollUsingChapter(DungeonMode.Chapter chapter)
    {
        _currentPos = SetPos();

        _targetIndex = (int)chapter;
        _targetPos = _points[_targetIndex];

        _scaleChangeTimer.Reset();
        _scaleChangeTimer.Start(1f);
    }

    void ScaleTarget(float ratio = 1)
    {
        for (int i = 0; i < contentTr.childCount; i++)
        {
            if (i == _targetIndex)
            {
                Transform targetTr = contentTr.GetChild(i);
                targetTr.localScale = Vector3.Lerp(targetTr.localScale, Vector3.one * _selectedSize, ratio);
            }
            else
            {
                Transform targetTr = contentTr.GetChild(i);
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
