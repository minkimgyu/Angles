using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class CostCardViewer : CardViewer
{
    [SerializeField] TMP_Text _costText;

    public override void Initialize(SKillCardData cardData, Action OnClick)
    {
        base.Initialize(cardData, OnClick);
        _costText.text = cardData.Cost.ToString();
    }
}
