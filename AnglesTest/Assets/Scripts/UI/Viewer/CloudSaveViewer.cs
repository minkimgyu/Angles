using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloudSaveViewer : BaseViewer
{
    [SerializeField] TMP_Text _infoTxt;
    [SerializeField] Image _saveIcon;

    const float _lockAlpha = 0.05f;

    public void Lock()
    {
        ChangeInfo("");
        _saveIcon.color = new Color(1, 1, 1, _lockAlpha);
    }

    public void ChangeIcon(Sprite icon)
    {
        _saveIcon.sprite = icon;
    }

    public void ChangeInfo(string info)
    {
        _infoTxt.text = info;
        if (_infoTxt.text == "") _infoTxt.gameObject.SetActive(false);
        else _infoTxt.gameObject.SetActive(true);
    }
}
