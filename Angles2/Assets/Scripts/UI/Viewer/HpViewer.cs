using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpViewer : BaseViewer
{
    [SerializeField] Image _bar;
    [SerializeField] float _changeDuration;
    [SerializeField] float _topOffset;

    [SerializeField] Color _startColor;
    [SerializeField] Color _endColor;

    IFollowable _followTarget;

    public override void Initialize()
    {
        _bar.color = _startColor;
    }

    public override void UpdateViewer(float ratio)
    {
        _bar.DOFillAmount(ratio, _changeDuration);

        Color targetColor = Color.Lerp(_startColor, _endColor, 1f - ratio);
        _bar.DOColor(targetColor, _changeDuration);
    }

    public override void SetFollower(IFollowable followTarget)
    {
        _followTarget = followTarget;
    }

    private void Update()
    {
        Vector3 offset = Vector2.down * _topOffset;
        transform.position = _followTarget.ReturnPosition() + offset;
    }

    private void OnDestroy()
    {
        _bar.DOKill();
    }
}