using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LobbyScrollController : ScrollUI
{
    [SerializeField] Transform _content;
    [SerializeField] Button _leftButton, _rightButton;

    public void Initialize()
    {
        _leftButton.onClick.AddListener(() => TabClick(true));
        _rightButton.onClick.AddListener(() => TabClick(false));

        int childCount = _content.childCount;
        for (int i = 0; i < childCount; i++)
        {
            RectTransform rectTransform = _content.GetChild(i).GetComponent<RectTransform>();

            // 현재 sizeDelta 값을 가져옴 (현재 크기의 높이는 유지하고 너비만 수정)
            Vector2 newSize = rectTransform.sizeDelta;
            newSize.x = Screen.width;  // 너비 설정
            newSize.y = Screen.height;  // 너비 설정

            rectTransform.sizeDelta = newSize;  // 변경된 값을 다시 할당
        }

        SetUp(childCount);
    }    

    public void TabClick(bool isLeft)
    {
        _currentPos = SetPos();
        int childCount = _content.childCount;

        if (isLeft)
        {
            _targetIndex--;
            if (_targetIndex < 0) _targetIndex = childCount - 1;
        }
        else
        {
            _targetIndex = ++_targetIndex % childCount;
        }

        _targetPos = _points[_targetIndex];
    }
}