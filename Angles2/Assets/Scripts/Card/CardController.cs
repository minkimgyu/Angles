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

public class CardController : MonoBehaviour
{
    [SerializeField] GameObject _uiObject;
    [SerializeField] Transform _cardParent;

    [SerializeField] TMP_Text _coinTxt;
    [SerializeField] Button _backButton;


    [SerializeField] GameObject _recreateCountObject;
    [SerializeField] TMP_Text _recreateCountText;
    [SerializeField] Button _recreateButton;

    Dictionary<BaseSkill.Name, BaseViewer> _cardViewerDictionary;

    Dictionary<BaseSkill.Name, Sprite> _skillIcons;
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas;
    Dictionary<BaseSkill.Name, SkillData> CopySkillDatas;

    List<BaseSkill.Name> _upgradeableSkills;

    BaseFactory _viewerFactory;
    BaseFactory _skillFactory;

    public void Initialize(
        Dictionary<BaseSkill.Name, CardInfoData> cardDatas,
        List<BaseSkill.Name> upgradeableSkills, 
        Dictionary<BaseSkill.Name, SkillData> skillDatas,
        Dictionary<BaseSkill.Name, Sprite> skillIcons,

        BaseFactory viewerFactory,
        BaseFactory skillFactory
        )
    {
        _uiObject.SetActive(false);

        _cardViewerDictionary = new Dictionary<BaseSkill.Name, BaseViewer>();
        _upgradeableSkills = upgradeableSkills;
        CopySkillDatas = skillDatas;

        _skillIcons = skillIcons;
        _cardDatas = cardDatas;

        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.CreateCard, new CreateCardCommand(CreateCards));
        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.CreateReusableCard, new CreateReusableCardCommand(CreateCards));

        _viewerFactory = viewerFactory;
        _skillFactory = skillFactory;
        _backButton.onClick.AddListener(CloseTab);
    }

    void CloseTab()
    {
        ServiceLocater.ReturnTimeController().Restart();
        DeleteCards();
    }

    // maxUpgrade�� ��ų�� �������� �ʴ´�.
    public void CreateCards(ICaster caster, int cardCount)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(caster, cardCount);
        if (upgradeDatas.Count == 0) return;

        _uiObject.SetActive(true);

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
            AddCard(caster, cardData, BaseViewer.Name.CardViewer);
        }

        _recreateCountObject.SetActive(false);
        _recreateButton.gameObject.SetActive(false);

        _coinTxt.text = GameStateManager.Instance.ReturnCoin().ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_coinTxt.transform);

        ServiceLocater.ReturnTimeController().Stop();
    }

    // maxUpgrade�� ��ų�� �������� �ʴ´�.
    public void CreateCards(ICaster caster, int cardCount, int recreateCount)
    {
        List<SkillUpgradeData> upgradeDatas = ReturnUpgradeDatas(caster, cardCount);
        if (upgradeDatas.Count == 0) return;

        _uiObject.SetActive(true);

        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            BaseSkill.Name skillName = upgradeDatas[i].Name;
            int upgradeCount = upgradeDatas[i].UpgradeCount; // 0, 1, 2, 3, 4
            int maxUpgradeCount = upgradeDatas[i].MaxUpgradeCount; // 5

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

            AddCard(caster, cardData, BaseViewer.Name.CostCardViewer);
        }

        recreateCount -= 1;
        if (recreateCount > 0)
        {
            _recreateCountObject.SetActive(true);
            _recreateButton.gameObject.SetActive(true);

            _recreateCountText.text = recreateCount.ToString();

            _recreateButton.onClick.RemoveAllListeners(); // �̺�Ʈ ���� �� �ٽ� ���
            _recreateButton.onClick.AddListener(
                () =>
                {
                    ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Reroll);
                    DeleteCards();
                    CreateCards(caster, cardCount, recreateCount);
                }
            );
        }
        else
        {
            _recreateCountText.text = "";
            _recreateButton.onClick.RemoveAllListeners();

            _recreateCountObject.SetActive(false);
            _recreateButton.gameObject.SetActive(false);
        }

        _coinTxt.text = GameStateManager.Instance.ReturnCoin().ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_coinTxt.transform);

        ServiceLocater.ReturnTimeController().Stop();
    }

    List<SkillUpgradeData> ReturnUpgradeDatas(ICaster upgradeable, int maxCardCount)
    {
        // ���� 5 - 5 �̷� ������ ���׷��̵� �� ��ų���� �ľ��Ѵ� --> �� ģ���� ���ܽ�Ŵ
        // �ִ� ȹ��, ���׷��̵� ������ ��ų ������ ���Ѵ� --> �ִ� 3�� �׸��� ���ٸ� ī�带 �ٿ����� --> ü��+ ī�峪 ü�� ȸ�� ī�带 �־ �� ��?
        List<SkillUpgradeData> containedSkillUpgradeDatas = upgradeable.ReturnSkillUpgradeDatas();
        List<SkillUpgradeData> skillUpgradeDatas = new List<SkillUpgradeData>();

        for (int i = 0; i < _upgradeableSkills.Count; i++)
        {
            BaseSkill.Name skillName = _upgradeableSkills[i];

            bool nowMaxUpgrade = false;
            bool alreadyHave = false;
            for (int j = 0; j < containedSkillUpgradeDatas.Count; j++)
            {
                if (containedSkillUpgradeDatas[j].Name == skillName)
                {
                    alreadyHave = true;
                    if (containedSkillUpgradeDatas[j].CanUpgrade() == false) // ���� ���̰� ���׷��̵尡 �Ұ����� ���
                    {
                        nowMaxUpgrade = true;
                        break;
                    }

                    skillUpgradeDatas.Add(containedSkillUpgradeDatas[j]);
                    break;
                }
            }

            if(alreadyHave == true && nowMaxUpgrade == true) // �ѱ��.
            {
                continue;
            }
            else if (alreadyHave == false)
            {
                // ���� ������ 1�̱� ����
                SkillUpgradeData upgradeData = new SkillUpgradeData(skillName, 1, CopySkillDatas[skillName]._maxUpgradePoint);
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

    public void DeleteCards()
    {
        foreach (var item in _cardViewerDictionary)
        {
            Destroy(item.Value.gameObject);
        }

        // ��ųʸ� �ʱ�ȭ
        _cardViewerDictionary.Clear();
        _uiObject.SetActive(false);
    }

    //1 - ó���� 1 --> ó���� ���׷��̵� X
    //2 - ���׷��̵�
    //3 - ���׷��̵�
    //4 - ���׷��̵�
    //5 - ���׷��̵�

    //1 - ���׷��̵�
    //2 - ���׷��̵�
    //3 - ���׷��̵�

    void PickCard(ICaster caster, int cost, BaseSkill.Name name)
    {
        int currentCoinCount = GameStateManager.Instance.ReturnCoin();
        if (cost > currentCoinCount) return;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Upgrade);
        GameStateManager.Instance.ChangeCoin(-cost);
        CloseTab();

        BaseSkill skill = _skillFactory.Create(name);
        caster.AddSkill(name, skill);
    }

    void AddCard(ICaster caster, SKillCardData skillCardData, BaseViewer.Name type)
    {
        BaseViewer viewer = _viewerFactory.Create(type);
        viewer.transform.SetParent(_cardParent);

        viewer.Initialize
        (
            skillCardData,
            () => { PickCard(caster, skillCardData.Cost, skillCardData.Name); }
        );
        _cardViewerDictionary.Add(skillCardData.Name, viewer);
    }
}
