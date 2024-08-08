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
    [SerializeField] TMP_Text _infoText;

    [SerializeField] GameObject _costParent;
    [SerializeField] TMP_Text _costText;

    [SerializeField] List<GameObject> _upgradeIcons;
    Action OnClick;

    public override void Initialize(SKillCardData cardData, Action OnClick)
    {
        _nameText.text = cardData.Name.ToString();
        _iconImage.sprite = cardData.Icon;
        _infoText.text = cardData.Info;
        _costText.text = cardData.Cost.ToString();

        bool shotCost = cardData.Cost != 0;
        _costParent.SetActive(shotCost);

        for (int i = 0; i < cardData.MaxUpgradeCount; i++)
        {
            if (i > cardData.UpgradeCount) _upgradeIcons[i].SetActive(false);
            else _upgradeIcons[i].SetActive(true);
        }

        this.OnClick = OnClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
