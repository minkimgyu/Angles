using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkinViewer : BaseViewer, IPointerDownHandler
{
    [SerializeField] Image skinImg;
    [SerializeField] GameObject lockGO;
    Action OnClick;

    public void Initialize(Sprite statIcon, Action OnClick)
    {
        skinImg.sprite = statIcon;
        this.OnClick = OnClick;
    }

    public void ActivateLockImg(bool on)
    {
        lockGO.SetActive(on);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
