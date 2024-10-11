using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class CardViewer : BaseViewer, IPointerClickHandler
{
    [SerializeField] protected TMP_Text _nameText;
    [SerializeField] protected Image _iconImage;
    [SerializeField] protected TMP_Text _infoText;

    [SerializeField] List<GameObject> _upgradeIcons;
    Action OnClick;

    public override void Initialize(SKillCardData cardData, Action OnClick)
    {
        _nameText.text = cardData.Name.ToString();
        _iconImage.sprite = cardData.Icon;
        _infoText.text = cardData.Info;

        for (int i = 0; i < _upgradeIcons.Count; i++)
        {
            _upgradeIcons[i].SetActive(false);
        }

        Debug.Log(cardData.UpgradeCount);
        Debug.Log(cardData.MaxUpgradeCount);

        for (int i = 0; i < cardData.UpgradeCount; i++)
        {
            _upgradeIcons[i].SetActive(true);
        }

        this.OnClick = OnClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
