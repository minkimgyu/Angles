using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LaserEffect : BaseEffect
{
    LineRenderer _line;
    [SerializeField] float _maxWidth;

    [SerializeField] float _startDuration;
    [SerializeField] float _endDuration;

    [SerializeField] Color _startColor;
    [SerializeField] Color _endColor;

    public override void Initialize()
    {
        base.Initialize();
        _line = GetComponent<LineRenderer>();
    }

    public override void ResetLine(Vector3 startPoint, Vector3 endPoint) 
    {
        // 라인렌더러의 색을 지정해준다.
        _line.startColor = _startColor;
        _line.endColor = _startColor;

        _line.positionCount = 2;
        _line.SetPosition(0, startPoint);
        _line.SetPosition(1, endPoint);
    }

    public override void ResetColor(Color startColor, Color endColor) 
    {
        _startColor = startColor;
        _endColor = endColor;
    }

    void ChangeWidth(float width, Color startColor, Color endColor, float duration, Action WhenCompleted)
    {
        _line.DOColor(new Color2(startColor, startColor), new Color2(endColor, endColor), duration);

        Tweener tweener = DOTween.To(() => _line.startWidth, x => _line.startWidth = x, width, duration);
        tweener.OnComplete(() => { WhenCompleted?.Invoke(); });
    }

    public override void Play()
    {
        ChangeWidth(_maxWidth, _startColor, _endColor, _startDuration,
            () => ChangeWidth(0, _endColor, _startColor, _endDuration, DestoryAfterDelay));
    }
}
