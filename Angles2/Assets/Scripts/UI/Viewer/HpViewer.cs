using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpViewer : MonoBehaviour
{
    [SerializeField] Image _bar;
    [SerializeField] float _changeDuration;
    [SerializeField] float _topOffset;

    [SerializeField] Color _startColor;
    [SerializeField] Color _endColor;

    IFollowable _followTarget;

    public void Initialize()
    {
        _bar.color = _startColor;
    }

    public void OnHpChange(float ratio)
    {
        _bar.DOFillAmount(ratio, _changeDuration);

        Color targetColor = Color.Lerp(_startColor, _endColor, 1f - ratio);
        _bar.DOColor(targetColor, _changeDuration);
    }

    public void SetTracker(IFollowable followTarget)
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
