using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class TextEffect : BaseEffect
{
    TMP_Text _text;
    [SerializeField] float _fadeInDuration = 0.7f;
    [SerializeField] float _fadeOutDuration = 1.5f;

    public override void Initialize()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    public override void ResetText(float damage)
    {
        _text.alpha = 0;
        _text.transform.localScale = Vector3.one;

        _text.text = damage.ToString();
    }

    void Fade(float alpha, float scale, Action WhenCompleted)
    {
        _text.transform.DOScale(Vector3.one * scale, _fadeInDuration);
        _text.DOFade(alpha, _fadeInDuration).OnComplete(() => WhenCompleted?.Invoke());
    }

    public override void Play()
    {
        Fade(1, 1.3f, () => Fade(0, 0.8f, DestoryAfterDelay));
    }
}
