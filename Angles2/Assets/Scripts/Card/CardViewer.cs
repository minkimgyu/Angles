using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class CardViewer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text _nameText;
    [SerializeField] Image _iconImage;
    [SerializeField] List<GameObject> _upgradeIcons;
    [SerializeField] TMP_Text _infoText;

    Action OnClick;

    public void Initialize(SKillCardData cardData, Action OnClick)
    {
        _nameText.text = cardData.Name.ToString();
        _iconImage.sprite = cardData.Icon;

        for (int i = 0; i < _upgradeIcons.Count; i++)
        {
            if (i >= cardData.MaxUpgradeCount) continue;

            if (i < cardData.UpgradeCount) _upgradeIcons[i].SetActive(true);
            else _upgradeIcons[i].SetActive(false);
        }

        _infoText.text = cardData.Info;
        this.OnClick = OnClick;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
