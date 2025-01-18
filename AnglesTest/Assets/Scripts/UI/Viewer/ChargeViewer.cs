using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeViewer : BaseViewer
{
    [SerializeField] Image _fillImg;

    public override void Initialize() 
    {
        _fillImg.fillAmount = 0;
    }

    public void UpdateChargeRatio(float ratio) 
    {
        _fillImg.fillAmount = ratio;
    }

    public void UpdateChargeAlpha(float alpha)
    {
        _fillImg.color = new Color(_fillImg.color.r, _fillImg.color.g, _fillImg.color.b, alpha);
    }
}
