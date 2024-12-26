using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinGaugeViewer : RatioViewer
{
    [SerializeField] TMP_Text _levelText;
    [SerializeField] TMP_Text _needCoinText;

    public void UpdateLevel(int ratio)
    {
        _levelText.text = $"Lv.{ratio}";
    }

    public void UpdateNeedCoin(Tuple<int, int> needCoin)
    {
        _needCoinText.text = $"{needCoin.Item1} / {needCoin.Item2}";
    }
}
