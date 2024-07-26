using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class CardViewer : BaseViewer, IPointerClickHandler
{
    [SerializeField] TMP_Text _nameText;
    [SerializeField] Image _iconImage;
    [SerializeField] Transform _upgradeImageParent;
    [SerializeField] TMP_Text _infoText;

    Action OnClick;

    public override void Initialize(SKillCardData cardData, Action OnClick)
    {
        _nameText.text = cardData.Name.ToString();
        _iconImage.sprite = cardData.Icon;
        _infoText.text = cardData.Info;
        this.OnClick = OnClick;
    }

    public override void AddChildUI(Transform child)
    {
        child.SetParent(_upgradeImageParent);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
