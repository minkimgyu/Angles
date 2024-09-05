using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

// 여기에 코스트 추가하기 --> 만약 코스트가 따로 할당되지 않는다면 코인을 소모하지 않는다.
public struct SKillCardData
{
    public SKillCardData(BaseSkill.Name name, Sprite icon, string info, int upgradeCount, int maxUpgradeCount)
    {
        _name = name;
        _icon = icon;
        _info = info;
        _cost = 0;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    public SKillCardData(BaseSkill.Name name, Sprite icon, string info, int cost, int upgradeCount, int maxUpgradeCount)
    {
        _name = name;
        _icon = icon;
        _info = info;
        _cost = cost;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    BaseSkill.Name _name;
    public BaseSkill.Name Name { get { return _name; } }

    Sprite _icon;
    public Sprite Icon { get { return _icon; } }

    string _info;
    public string Info { get { return _info; } }

    int _cost;
    public int Cost { get { return _cost; } }

    int _upgradeCount;
    public int UpgradeCount { get { return _upgradeCount; } }

    int _maxUpgradeCount;
    public int MaxUpgradeCount { get { return _maxUpgradeCount; } }

}

public class CardUIController : MonoBehaviour
{
    [SerializeField] GameObject _uiObject;
    [SerializeField] Transform _cardParent;
    [SerializeField] Image _backPanel;

    [SerializeField] Button _backButton;

    [SerializeField] TMP_Text _titleText;

    [SerializeField] GameObject _recreateCountParent;
    [SerializeField] TMP_Text _recreateCountText;
    [SerializeField] Button _recreateButton;

    Dictionary<BaseSkill.Name, BaseViewer> _cardViewerDictionary;

    Dictionary<BaseSkill.Name, Sprite> _skillIcons;
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas;
    Dictionary<BaseSkill.Name, BaseSkillData> _skillDatas;

    List<BaseSkill.Name> _upgradeableSkills;

    BaseFactory _viewerFactory;
    BaseFactory _skillFactory;
    ISkillUser _skillUser;

    public void Initialize(
        Dictionary<BaseSkill.Name, CardInfoData> cardDatas,
        List<BaseSkill.Name> upgradeableSkills, 
        Dictionary<BaseSkill.Name, BaseSkillData> skillDatas,
        Dictionary<BaseSkill.Name, Sprite> skillIcons,

        BaseFactory viewerFactory,
        BaseFactory skillFactory
        )
    {
        _uiObject.SetActive(false);

        _cardViewerDictionary = new Dictionary<BaseSkill.Name, BaseViewer>();
        _upgradeableSkills = upgradeableSkills;
        _skillDatas = skillDatas;

        _skillIcons = skillIcons;
        _cardDatas = cardDatas;


        SubEventBus.Register(SubEventBus.State.RegisterSkillUpgradeable, new RegisterSkillUpgradeableCommand((value) => _skillUser = value));
        SubEventBus.Register(SubEventBus.State.CreateCard, new CreateCardCommand(CreateCards));
        SubEventBus.Register(SubEventBus.State.CreateReusableCard, new CreateReusableCardCommand(CreateCards));

        _viewerFactory = viewerFactory;
        _skillFactory = skillFactory;
        _backButton.onClick.AddListener(CloseTab);
    }

    void CloseTab()
    {
        Time.timeScale = 1; 
        DeleteCards();
    }

    // maxUpgrade인 스킬은 포함하지 않는다.
    public void CreateCards(int cardCount)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(cardCount);

        if (upgradeDatas.Count == 0) return;
        Time.timeScale = 0;

        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            BaseSkill.Name skillName = upgradeDatas[i].Name;
            int upgradeCount = upgradeDatas[i].UpgradeCount;
            int maxUpgradeCount = upgradeDatas[i].MaxUpgradeCount;

            CardInfoData cardInfoData = _cardDatas[skillName];
            Sprite cardIcon = _skillIcons[cardInfoData.IconName];
            string info = cardInfoData.Info;

            SKillCardData cardData = new SKillCardData(
                skillName,
                cardIcon,
                info,
                upgradeCount,
                maxUpgradeCount
            );
            AddCard(cardData, BaseViewer.Name.CardViewer);
        }

        _recreateCountParent.gameObject.SetActive(false);

        _uiObject.SetActive(true);
        _backPanel.DOFade(0.5f, 2);
    }

    // maxUpgrade인 스킬은 포함하지 않는다.
    public void CreateCards(int cardCount, int recreateCount)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(cardCount);

        if (upgradeDatas.Count == 0) return;
        Time.timeScale = 0;

        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            BaseSkill.Name skillName = upgradeDatas[i].Name;
            int upgradeCount = upgradeDatas[i].UpgradeCount;
            int maxUpgradeCount = upgradeDatas[i].MaxUpgradeCount;

            CardInfoData cardInfoData = _cardDatas[skillName];
            Sprite cardIcon = _skillIcons[cardInfoData.IconName];
            string info = cardInfoData.Info;

            SKillCardData cardData = new SKillCardData(
                skillName,
                cardIcon,
                info,
                cardInfoData.Cost,
                upgradeCount,
                maxUpgradeCount
            );

            AddCard(cardData, BaseViewer.Name.CostCardViewer);
        }

        recreateCount -= 1;
        if (recreateCount > 0)
        {
            _recreateCountParent.gameObject.SetActive(true);
            _recreateCountText.text = recreateCount.ToString();

            _recreateButton.onClick.RemoveAllListeners(); // 이벤트 제거 후 다시 등록
            _recreateButton.onClick.AddListener(
                () =>
                {
                    DeleteCards();
                    CreateCards(cardCount, recreateCount);
                }
            );
        }
        else
        {
            _recreateCountParent.gameObject.SetActive(false);
            _recreateCountText.text = "";
            _recreateButton.onClick.RemoveAllListeners();
        }

        _uiObject.SetActive(true);
        _backPanel.DOFade(0.5f, 2);
    }

    List<SkillUpgradeData> ReturnUpgradeDatas(int maxCardCount)
    {
        // 먼저 5 - 5 이런 끝까지 업그레이드 된 스킬들을 파악한다 --> 이 친구는 제외시킴
        // 최대 획득, 업그레이드 가능한 스킬 개수를 구한다 --> 최대 3개 그리고 없다면 카드를 줄여야함 --> 체력+ 카드나 체력 회복 카드를 넣어도 될 듯?
        List<SkillUpgradeData> containedSkillUpgradeDatas = _skillUser.ReturnSkillUpgradeDatas();

        List<SkillUpgradeData> skillUpgradeDatas = new List<SkillUpgradeData>();

        for (int i = 0; i < _upgradeableSkills.Count; i++)
        {
            BaseSkill.Name skillName = _upgradeableSkills[i];

            bool alreadyHave = false;
            for (int j = 0; j < containedSkillUpgradeDatas.Count; j++)
            {
                if (containedSkillUpgradeDatas[j].Name == skillName && containedSkillUpgradeDatas[j].UpgradeCount != containedSkillUpgradeDatas[j].MaxUpgradeCount)
                {
                    skillUpgradeDatas.Add(containedSkillUpgradeDatas[j]);
                    alreadyHave = true;
                    break;
                }
            }

            if (alreadyHave == false)
            {
                SkillUpgradeData upgradeData = new SkillUpgradeData(_upgradeableSkills[i], 0, _skillDatas[skillName]._maxUpgradePoint);
                skillUpgradeDatas.Add(upgradeData);
            }
        }


        if (skillUpgradeDatas.Count >= maxCardCount)
        {
            List<SkillUpgradeData> results = new List<SkillUpgradeData>();

            while (results.Count < maxCardCount) // 중복 검출해주기
            {
                int randomIndex = UnityEngine.Random.Range(0, skillUpgradeDatas.Count);
                if (results.Contains(skillUpgradeDatas[randomIndex]) == false) results.Add(skillUpgradeDatas[randomIndex]);
            }

            return results;
        }
        else
        {
            return skillUpgradeDatas;
        }
    }

    public void AddSkillUser(ISkillUser skillUser)
    {
        _skillUser = skillUser;
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

    void PickCard(int cost, BaseSkill.Name name)
    {
        int currentCoinCount;
        GameStateEventBus.Publish(GameStateEventBus.State.ReturnCoin, out currentCoinCount);
        if (cost > currentCoinCount) return;

        GameStateEventBus.Publish(GameStateEventBus.State.ChangeCoin, -cost);
        CloseTab();

        BaseSkill skill = _skillFactory.Create(name);
        _skillUser.AddSkill(name, skill);
    }

    void AddCard(SKillCardData skillCardData, BaseViewer.Name type)
    {
        BaseViewer viewer = _viewerFactory.Create(type);
        viewer.transform.SetParent(_cardParent);

        viewer.Initialize
        (
            skillCardData,
            () => { PickCard(skillCardData.Cost, skillCardData.Name); }
        );
        _cardViewerDictionary.Add(skillCardData.Name, viewer);
    }
}
