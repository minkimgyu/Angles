using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class StatViewer : BaseViewer, IPointerDownHandler
{
    [SerializeField] Image statImg;
    Action OnClick;

    public void Initialize(Sprite statIcon, Action OnClick)
    {
        statImg.sprite = statIcon;
        this.OnClick = OnClick;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
