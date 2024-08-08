using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StageResultViewer : BaseViewer
{
    [SerializeField] TMP_Text _stageResultTxt;
    [SerializeField] Image _leftImg;
    [SerializeField] Image _rightImg;
    float _fadeDuration = 2;

    public override void TurnOnViewer(bool show)
    {
        _stageResultTxt.DOKill();
        _leftImg.DOKill();
        _rightImg.DOKill();

        if (show == false)
        {
            _stageResultTxt.alpha = 0;
            _leftImg.color = new Color(_leftImg.color.r, _leftImg.color.g, _leftImg.color.b, 0);
            _rightImg.color = new Color(_leftImg.color.r, _leftImg.color.g, _leftImg.color.b, 0);
        }
        else
        {
            _stageResultTxt.alpha = 1;
            _leftImg.color = new Color(_leftImg.color.r, _leftImg.color.g, _leftImg.color.b, 1);
            _rightImg.color = new Color(_leftImg.color.r, _leftImg.color.g, _leftImg.color.b, 1);
        }
    }

    public override void UpdateViewer(string info)
    {
        _stageResultTxt.text = info;

        _stageResultTxt.DOFade(1, _fadeDuration).OnComplete(() => { _stageResultTxt.DOFade(0, _fadeDuration); });
        _leftImg.DOFade(1, _fadeDuration).OnComplete(() => { _leftImg.DOFade(0, _fadeDuration); });
        _rightImg.DOFade(1, _fadeDuration).OnComplete(() => { _rightImg.DOFade(0, _fadeDuration); });
    }
}
