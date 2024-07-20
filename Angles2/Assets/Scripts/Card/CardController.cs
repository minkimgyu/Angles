using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public struct SKillCardData
{
    public SKillCardData(BaseSkill.Name name, int upgradeCount, int maxUpgradeCount)
    {
        _name = name;
        _icon = null;
        _info = null;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    BaseSkill.Name _name;
    public BaseSkill.Name Name { get { return _name; } }

    Sprite _icon;
    public Sprite Icon { get { return _icon; } }

    string _info;
    public string Info { get { return _info; } }

    int _upgradeCount;
    public int UpgradeCount { get { return _upgradeCount; } }

    int _maxUpgradeCount;
    public int MaxUpgradeCount { get { return _maxUpgradeCount; } }

}

public class CardController : MonoBehaviour
{
    [SerializeField] GameObject _uiObject;
    [SerializeField] Transform _cardParent;
    [SerializeField] CardViewer _cardPrefab;

    [SerializeField] Image _backPanel;

    //[SerializeField] CardInfoDictionary _cardInfoDictionary;
    Dictionary<BaseSkill.Name, CardViewer> _cardViewerDictionary;

    Action<BaseSkill.Name> ReturnSkillNameToAdd;

    [SerializeField] int maxCardCount = 3;

    public void Initialize(Action<BaseSkill.Name> ReturnSkillNameToAdd)
    {
        _cardViewerDictionary = new Dictionary<BaseSkill.Name, CardViewer>();
        this.ReturnSkillNameToAdd = ReturnSkillNameToAdd;
    }

    // maxUpgrade인 스킬은 포함하지 않는다.
    public void CreateCards(List<SkillUpgradeData> skillUpgradeDatas)
    {
        int cardCount = 0;

        //foreach (var item in _cardInfoDictionary)
        //{
        //    if (cardCount == maxCardCount) break;
        //    BaseSkill.Name name = item.Key;

        //    for (int i = 0; i < skillUpgradeDatas.Count; i++)
        //    {
        //        if (name != skillUpgradeDatas[i].Name) continue;

        //        int upgradeCount = skillUpgradeDatas[i].UpgradeCount;
        //        int maxUpgradeCount = skillUpgradeDatas[i].MaxUpgradeCount;
        //        if (upgradeCount == maxUpgradeCount) continue;

        //        cardCount++;

        //        SKillCardData cardData = new SKillCardData(
        //            skillUpgradeDatas[i].Name,
        //            _cardInfoDictionary[skillUpgradeDatas[i].Name],
        //            skillUpgradeDatas[i].UpgradeCount,
        //            skillUpgradeDatas[i].MaxUpgradeCount
        //        );

        //        AddCard(cardData);
        //    }
        //}

        _uiObject.SetActive(true);
        _backPanel.DOFade(1, 2);
    }

    public void DeleteCards()
    {
        foreach (var item in _cardViewerDictionary)
        {
            RemoveCard(item.Key);
        }

        _uiObject.SetActive(false);
        _backPanel.DOFade(0, 2);
    }

    void PickCard(BaseSkill.Name name)
    {
        ReturnSkillNameToAdd?.Invoke(name);
        DeleteCards();
    }

    void AddCard(SKillCardData skillCardData)
    {
        CardViewer viewer = Instantiate(_cardPrefab, _cardParent);
        viewer.Initialize(
            skillCardData,
            () => { PickCard(skillCardData.Name); }
        );
        _cardViewerDictionary.Add(skillCardData.Name, viewer);
    }
    void RemoveCard(BaseSkill.Name name)
    {
        CardViewer viewer = _cardViewerDictionary[name];
        Destroy(viewer.gameObject);

        _cardViewerDictionary.Remove(name);
    }
}
