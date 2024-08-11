using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

// ���⿡ �ڽ�Ʈ �߰��ϱ� --> ���� �ڽ�Ʈ�� ���� �Ҵ���� �ʴ´ٸ� ������ �Ҹ����� �ʴ´�.
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

    Func<BaseViewer.Name, BaseViewer> SpawnViewer;

    Action<int> ChangeCoin;
    Func<int> ReturnCoin;

    ISkillUser _skillUsable;

    public void Initialize(Dictionary<BaseSkill.Name, CardInfoData> cardDatas,
        List<BaseSkill.Name> upgradeableSkills, Dictionary<BaseSkill.Name, BaseSkillData> skillDatas,

        Dictionary<BaseSkill.Name, Sprite> skillIcons,
        Func<BaseViewer.Name, BaseViewer> SpawnViewer,

        Action<int> ChangeCoin,
        Func<int> ReturnCoin
        )
    {
        _uiObject.SetActive(false);

        _cardViewerDictionary = new Dictionary<BaseSkill.Name, BaseViewer>();
        _upgradeableSkills = upgradeableSkills;
        _skillDatas = skillDatas;

        _skillIcons = skillIcons;
        _cardDatas = cardDatas;

        this.SpawnViewer = SpawnViewer;

        this.ChangeCoin = ChangeCoin;
        this.ReturnCoin = ReturnCoin;
        _backButton.onClick.AddListener(CloseTab);
    }

    void CloseTab()
    {
        Time.timeScale = 1; 
        DeleteCards();
    }

    // maxUpgrade�� ��ų�� �������� �ʴ´�.
    public void CreateCards(int cardCount, List<SkillUpgradeData> ContainedSkillUpgradeDatas)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(cardCount, ContainedSkillUpgradeDatas);

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
            AddCard(cardData);
        }

        _recreateCountParent.gameObject.SetActive(false);

        _uiObject.SetActive(true);
        _backPanel.DOFade(0.5f, 2);
    }

    // maxUpgrade�� ��ų�� �������� �ʴ´�.
    public void CreateCards(int cardCount, int recreateCount, List<SkillUpgradeData> ContainedSkillUpgradeDatas)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(cardCount, ContainedSkillUpgradeDatas);

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

            AddCard(cardData);
        }

        recreateCount -= 1;
        if (recreateCount > 0)
        {
            _recreateCountParent.gameObject.SetActive(true);
            _recreateCountText.text = recreateCount.ToString();

            _recreateButton.onClick.RemoveAllListeners(); // �̺�Ʈ ���� �� �ٽ� ���
            _recreateButton.onClick.AddListener(
                () =>
                {
                    DeleteCards();
                    ReCreateCards(cardCount, recreateCount, ContainedSkillUpgradeDatas);
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

    void ReCreateCards(int maxCardCount, int maxRecreateCount, List<SkillUpgradeData> ContainedSkillUpgradeDatas) => CreateCards(maxCardCount, maxRecreateCount, ContainedSkillUpgradeDatas);

    List<SkillUpgradeData> ReturnUpgradeDatas(int maxCardCount, List<SkillUpgradeData> ContainedSkillUpgradeDatas)
    {
        // ���� 5 - 5 �̷� ������ ���׷��̵� �� ��ų���� �ľ��Ѵ� --> �� ģ���� ���ܽ�Ŵ
        // �ִ� ȹ��, ���׷��̵� ������ ��ų ������ ���Ѵ� --> �ִ� 3�� �׸��� ���ٸ� ī�带 �ٿ����� --> ü��+ ī�峪 ü�� ȸ�� ī�带 �־ �� ��?

        List<SkillUpgradeData> skillUpgradeDatas = new List<SkillUpgradeData>();

        for (int i = 0; i < _upgradeableSkills.Count; i++)
        {
            BaseSkill.Name skillName = _upgradeableSkills[i];

            bool alreadyHave = false;
            for (int j = 0; j < ContainedSkillUpgradeDatas.Count; j++)
            {
                if (ContainedSkillUpgradeDatas[j].Name == skillName && ContainedSkillUpgradeDatas[j].UpgradeCount != ContainedSkillUpgradeDatas[j].MaxUpgradeCount)
                {
                    skillUpgradeDatas.Add(ContainedSkillUpgradeDatas[j]);
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

            while (results.Count < maxCardCount) // �ߺ� �������ֱ�
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

    public void AddSkillUser(ISkillUser skillUsable)
    {
        _skillUsable = skillUsable;
    }

    public void DeleteCards()
    {
        foreach (var item in _cardViewerDictionary)
        {
            Destroy(item.Value.gameObject);
        }

        // ��ųʸ� �ʱ�ȭ
        _cardViewerDictionary.Clear();

        Debug.Log(_cardViewerDictionary.Count);

        _uiObject.SetActive(false);
        _backPanel.DOFade(0, 2);
    }

    void PickCard(int cost, BaseSkill.Name name)
    {
        int coin = ReturnCoin();
        if (cost > coin) return;

        ChangeCoin?.Invoke(coin - cost);
        CloseTab();
        _skillUsable.AddSkill(name);
    }

    void AddCard(SKillCardData skillCardData)
    {
        BaseViewer viewer = SpawnViewer?.Invoke(BaseViewer.Name.CardViewer);
        viewer.transform.SetParent(_cardParent);

        viewer.Initialize
        (
            skillCardData,
            () => { PickCard(skillCardData.Cost, skillCardData.Name); }
        );
        _cardViewerDictionary.Add(skillCardData.Name, viewer);
    }
}