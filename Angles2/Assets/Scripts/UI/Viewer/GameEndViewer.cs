using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class GameEndViewer : BaseViewer
{
    [SerializeField] GameObject _viwerObject;

    [SerializeField] Image _background;
    [SerializeField] TMP_Text _labelTxt;
    [SerializeField] Image _labelImg;
    [SerializeField] Button _returnToMenuBtn;

    public override void Initialize(Action OnReturnToMenuRequested) 
    {
        _returnToMenuBtn.onClick.AddListener(() => { OnReturnToMenuRequested?.Invoke(); });
        _viwerObject.SetActive(false);

        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
        _labelImg.transform.localScale = new Vector3(1, 0, 1);
        _labelTxt.text = "";
    }

    public override void TurnOnViewer(bool show, float backgroundFadeRatio, float backgroundFadeDuration, string endInfo, Color labelColor, Color labelTxtColor) 
    {
        _viwerObject.SetActive(show);

        if (show == true)
        {
            _background.DOFade(backgroundFadeRatio, backgroundFadeDuration);

            _labelImg.transform.DOScale(new Vector3(1, 1, 1), 0.1f);
            _labelTxt.text = endInfo;
            _labelTxt.color = labelTxtColor;
            _labelImg.color = labelColor;
        }
    }
}
