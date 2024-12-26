using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RatioViewer : BaseViewer
{
    [SerializeField] protected Image _bar;
    [SerializeField] protected float _changeDuration;

    [SerializeField] protected Color _startColor;
    [SerializeField] protected Color _endColor;

    public override void Initialize()
    {
        _bar.color = _startColor;
    }

    public void UpdateRatio(float ratio) 
    {
        _bar.DOFillAmount(ratio, _changeDuration);

        Color targetColor = Color.Lerp(_startColor, _endColor, 1f - ratio);
        _bar.DOColor(targetColor, _changeDuration);
    }

    private void OnDestroy()
    {
        _bar.DOKill();
    }
}
