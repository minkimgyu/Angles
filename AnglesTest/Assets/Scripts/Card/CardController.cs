using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Skill;

// 여기에 코스트 추가하기 --> 만약 코스트가 따로 할당되지 않는다면 코인을 소모하지 않는다.
public struct SKillCardData
{
    public SKillCardData(BaseSkill.Name key, Sprite icon, string name, string info, int upgradeCount, int maxUpgradeCount)
    {
        _key = key;
        _icon = icon;

        _name = name;
        _info = info;
        _cost = 0;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    public SKillCardData(BaseSkill.Name key, Sprite icon, string name, string info, int cost, int upgradeCount, int maxUpgradeCount)
    {
        _key = key;
        _icon = icon;

        _name = name;
        _info = info;
        _cost = cost;
        _upgradeCount = upgradeCount;
        _maxUpgradeCount = maxUpgradeCount;
    }

    BaseSkill.Name _key;
    public BaseSkill.Name Key { get { return _key; } }

    Sprite _icon;
    public Sprite Icon { get { return _icon; } }


    string _name;
    public string Name { get { return _name; } }

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

    [SerializeField] TMP_Text _upgradeText;

    [SerializeField] GameObject _recreateCountObject;
    [SerializeField] TMP_Text _recreateInfoText;
    [SerializeField] TMP_Text _recreateCountText;
    [SerializeField] Button _recreateButton;

    Dictionary<BaseSkill.Name, BaseViewer> _cardViewerDictionary;

    Dictionary<BaseSkill.Name, Sprite> _skillIcons;
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas;
    Dictionary<BaseSkill.Name, SkillData> CopySkillDatas;

    HashSet<BaseSkill.Name> _upgradeableSkills;

    BaseFactory _viewerFactory;
    BaseFactory _skillFactory;

    public void Initialize(
        Dictionary<BaseSkill.Name, CardInfoData> cardDatas,
        HashSet<BaseSkill.Name> upgradeableSkills, 
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

        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.CreateCard, new CreateReusableCardCommand(CreateCards));
        //EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.CreateReusableCard, new CreateCardCommand(CreateCards));

        _viewerFactory = viewerFactory;
        _skillFactory = skillFactory;

        _upgradeText.GetComponentInChildren<TMP_Text>().text =
            ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Upgrade);

        _recreateButton.GetComponentInChildren<TMP_Text>().text = 
            ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.PickAgain);

        _recreateInfoText.GetComponentInChildren<TMP_Text>().text =
            $"{ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.LeftCount)}:";
    }

    void CloseTab()
    {
        ServiceLocater.ReturnTimeController().Restart();
        DeleteCards();
    }

    // maxUpgrade인 스킬은 포함하지 않는다.
    public void CreateCards(ICaster caster, int cardCount, int recreateCount)
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
            Sprite cardIcon = _skillIcons[skillName];

            string name = ServiceLocater.ReturnLocalizationHandler().GetWord($"{skillName.ToString()}CardName");
            string description = ServiceLocater.ReturnLocalizationHandler().GetWord($"{skillName.ToString()}CardDescription");

            SKillCardData cardData = new SKillCardData(
                skillName,
                cardIcon,

                name,
                description,
                upgradeCount,
                maxUpgradeCount
            );
            AddCard(caster, cardData, BaseViewer.Name.CardViewer);
        }

        if (recreateCount > 0)
        {
            _recreateCountObject.SetActive(true);
            _recreateButton.gameObject.SetActive(true);

            _recreateCountText.text = recreateCount.ToString();

            _recreateButton.onClick.RemoveAllListeners(); // 이벤트 제거 후 다시 등록
            _recreateButton.onClick.AddListener(
                () =>
                {
                    ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Reroll);
                    DeleteCards();

                    recreateCount -= 1;
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

        ServiceLocater.ReturnTimeController().Stop();
    }

    List<SkillUpgradeData> ReturnUpgradeDatas(ICaster upgradeable, int maxCardCount)
    {
        // 먼저 5 - 5 이런 끝까지 업그레이드 된 스킬들을 파악한다 --> 이 친구는 제외시킴
        // 최대 획득, 업그레이드 가능한 스킬 개수를 구한다 --> 최대 3개 그리고 없다면 카드를 줄여야함 --> 체력+ 카드나 체력 회복 카드를 넣어도 될 듯?
        List<SkillUpgradeData> containedSkillUpgradeDatas = upgradeable.ReturnSkillUpgradeDatas();
        List<SkillUpgradeData> skillUpgradeDatas = new List<SkillUpgradeData>();

        List<BaseSkill.Name> upgradeableSkills = _upgradeableSkills.ToList();

        for (int i = 0; i < upgradeableSkills.Count; i++)
        {
            BaseSkill.Name skillName = upgradeableSkills[i];

            bool nowMaxUpgrade = false;
            bool alreadyHave = false;
            for (int j = 0; j < containedSkillUpgradeDatas.Count; j++)
            {
                if (containedSkillUpgradeDatas[j].Name == skillName)
                {
                    alreadyHave = true;
                    if (containedSkillUpgradeDatas[j].CanUpgrade() == false) // 보유 중이고 업그레이드가 불가능한 경우
                    {
                        nowMaxUpgrade = true;
                        break;
                    }

                    skillUpgradeDatas.Add(containedSkillUpgradeDatas[j]);
                    break;
                }
            }

            if(alreadyHave == true && nowMaxUpgrade == true) // 넘긴다.
            {
                continue;
            }
            else if (alreadyHave == false)
            {
                // 다음 레벨은 1이기 떄문
                SkillUpgradeData upgradeData = new SkillUpgradeData(skillName, 1, CopySkillDatas[skillName].MaxUpgradePoint);
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

    public void DeleteCards()
    {
        foreach (var item in _cardViewerDictionary)
        {
            Destroy(item.Value.gameObject);
        }

        // 딕셔너리 초기화
        _cardViewerDictionary.Clear();
        _uiObject.SetActive(false);
    }

    //1 - 처음이 1 --> 처음은 업그레이드 X
    //2 - 업그레이드
    //3 - 업그레이드
    //4 - 업그레이드
    //5 - 업그레이드

    //1 - 업그레이드
    //2 - 업그레이드
    //3 - 업그레이드

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
        CardViewer viewer = (CardViewer)_viewerFactory.Create(type);
        viewer.transform.SetParent(_cardParent);
        viewer.transform.localScale = Vector3.one;

        viewer.Initialize
        (
            skillCardData,
            () => { PickCard(caster, skillCardData.Cost, skillCardData.Key); }
        );
        _cardViewerDictionary.Add(skillCardData.Key, viewer);
    }
}
