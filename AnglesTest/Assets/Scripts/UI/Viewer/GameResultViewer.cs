using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class GameResultViewer : BaseViewer, IPointerClickHandler
{
    [SerializeField] TMP_Text _recordTxt;
    [SerializeField] TMP_Text _coinTxt;
    [SerializeField] TMP_Text _tabTxt;

    Action OnReturnToMenuRequested;

    public override void Initialize(Action OnReturnToMenuRequested) 
    {
        this.OnReturnToMenuRequested = OnReturnToMenuRequested;
        TurnOnViewer(false);

        _recordTxt.text = "";
        _coinTxt.text = "";
    }

    public void FadeInOutTabTxt()
    {
        _tabTxt.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
    }

    public void ChangeCoin(int coinCount)
    {
        _coinTxt.text = coinCount.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_coinTxt.transform);
    }

    public void ChangeRecord(float recordTime)
    {
        int minute = (int)recordTime / 60;
        int second = (int)recordTime % 60;

        _recordTxt.text = $"Record : {minute}:{second}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnReturnToMenuRequested?.Invoke();
    }
}
