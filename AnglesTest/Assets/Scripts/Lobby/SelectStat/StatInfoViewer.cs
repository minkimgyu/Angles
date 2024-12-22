using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatInfoViewer : MonoBehaviour
{
    [SerializeField] Image _iconImage;

    [SerializeField] TMP_Text _nameTxt;
    [SerializeField] TMP_Text _levelTxt;
    [SerializeField] TMP_Text _costTxt;
    [SerializeField] GameObject _costGo;

    [SerializeField] TMP_Text _descriptionTxt;
    [SerializeField] GameObject _descriptionGo;

    public void ChangeStatImage(Sprite sprite)
    {
        _iconImage.sprite = sprite;
    }

    public void ChangeStatName(string name)
    {
        _nameTxt.text = name;
    }

    public void ChangeStatLevel(int level)
    {
        _levelTxt.text = $"Info: {level}";
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_levelTxt.transform.parent); // UI 레이아웃 즉시 강제 재설정
    }

    public void ChangeStatCost(int cost)
    {
        _costTxt.text = cost.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_costGo.transform.parent); // UI 레이아웃 즉시 강제 재설정
    }

    public void ChangeStatDescription(string info)
    {
        _descriptionTxt.text = info;
    }

    public void ActivateCost(bool on)
    {
        _costGo.SetActive(on);
    }

    public void ActivateDescription(bool on)
    {
        _descriptionGo.SetActive(on);
    }
}
