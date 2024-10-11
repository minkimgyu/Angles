using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterViewer : BaseViewer
{
    [SerializeField] Image _selectImg;
    [SerializeField] Image _lockImg;

    public override void Initialize(Sprite icon, bool isClear) 
    {
        _selectImg.sprite = icon;
        _lockImg.gameObject.SetActive(isClear);
    }
}
