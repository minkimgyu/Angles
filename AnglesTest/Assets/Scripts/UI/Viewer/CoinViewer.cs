using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinViewer : BaseViewer
{
    [SerializeField] TMP_Text _coinTxt;

    public void UpdateCoinCount(int coinCount)
    {
        _coinTxt.text = coinCount.ToString();
    }
}
