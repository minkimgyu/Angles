using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public struct SKillCardData
{
    public SKillCardData(BaseSkill.Name name, Sprite icon, string info, int upgradeCount, int maxUpgradeCount)
    {
        _name = name;
        _icon = icon;
        _info = info;
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

    [SerializeField] Image _backPanel;
    Dictionary<BaseSkill.Name, BaseViewer> _cardViewerDictionary;

    Action<BaseSkill.Name> ReturnSkillNameToAdd;

    int maxCardCount = 3;
    const string _icon = "Icon";

    public void Initialize(Action<BaseSkill.Name> ReturnSkillNameToAdd)
    {
        _cardViewerDictionary = new Dictionary<BaseSkill.Name, BaseViewer>();
        this.ReturnSkillNameToAdd = ReturnSkillNameToAdd;
    }

    // maxUpgrade인 스킬은 포함하지 않는다.
    public void CreateCards(List<SkillUpgradeData> skillUpgradeDatas)
    {
        Time.timeScale = 0;

        int dataCount = skillUpgradeDatas.Count;

        for (int i = 0; i < maxCardCount; i++)
        {
            int randomRange;
            BaseSkill.Name skillNameToAdd;

            //중복 검출해주기
            // 더 이상 스킬 카드를 만들 수 없다면 리턴해주기
            do
            {
                randomRange = UnityEngine.Random.Range(0, dataCount);
                skillNameToAdd = skillUpgradeDatas[randomRange].Name;
            }
            while (_cardViewerDictionary.ContainsKey(skillNameToAdd) == true);

            int upgradeCount = skillUpgradeDatas[randomRange].UpgradeCount;
            int maxUpgradeCount = skillUpgradeDatas[randomRange].MaxUpgradeCount;

            CardInfoData cardInfoData = Database.Instance.CardDatas[skillNameToAdd];
            Sprite cardIcon = AddressableManager.Instance.SpriteAssetDictionary[cardInfoData.IconName.ToString() + _icon];
            string info = cardInfoData.Info;

            SKillCardData cardData = new SKillCardData(
                skillNameToAdd,
                cardIcon,
                info,
                upgradeCount,
                maxUpgradeCount
            );

            AddCard(cardData);
        }

        _uiObject.SetActive(true);
        _backPanel.DOFade(0.5f, 2);
    }

    public void DeleteCards()
    {
        foreach (var item in _cardViewerDictionary)
        {
            Destroy(item.Value.gameObject);
        }

        // 딕셔너리 초기화
        _cardViewerDictionary.Clear();

        Debug.Log(_cardViewerDictionary.Count);

        _uiObject.SetActive(false);
        _backPanel.DOFade(0, 2);
    }

    void PickCard(BaseSkill.Name name)
    {
        Time.timeScale = 1;
        ReturnSkillNameToAdd?.Invoke(name);
        DeleteCards();
    }

    void AddCard(SKillCardData skillCardData)
    {
        BaseViewer viewer = ViewerFactory.Create(BaseViewer.Name.CardViewer);
        for (int i = 0; i < skillCardData.UpgradeCount; i++)
        {
            GameObject starUI = AddressableManager.Instance.PrefabAssetDictionary["StarViewer"];
            GameObject starObject = Instantiate(starUI);

            viewer.AddChildUI(starObject.transform);
        }

        viewer.transform.SetParent(_cardParent);

        viewer.Initialize(
            skillCardData,
            () => 
            { 
                PickCard(skillCardData.Name); 
            }
        );
        _cardViewerDictionary.Add(skillCardData.Name, viewer);
    }
}
