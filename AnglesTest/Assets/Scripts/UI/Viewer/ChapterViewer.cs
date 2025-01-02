using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterViewer : BaseViewer
{
    [SerializeField] Image _selectImg;
    [SerializeField] Image _lockImg;

    public void Initialize(Sprite icon, bool isUnlock) 
    {
        _selectImg.sprite = icon;
        _lockImg.gameObject.SetActive(!isUnlock);
    }
}
