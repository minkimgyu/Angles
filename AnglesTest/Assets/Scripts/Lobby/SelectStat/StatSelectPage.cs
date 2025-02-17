using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;

[System.Serializable]
public struct StatData
{
    public enum Key
    {
        AttackDamage,
        MoveSpeed,
        MaxHp,
        DamageReduction,
    }

    [JsonProperty] int _maxLevel;
    [JsonProperty] List<int> _cost;
    [JsonIgnore] public int MaxLevel { get => _maxLevel; }

    public int GetCost(int level) 
    { 
        if(level == MaxLevel) return 0;
        return _cost[level]; 
    }

    public string GetDescription(string description) 
    {
        return description; // 하나씩 앞선 값을 전달
    }

    public StatData(int maxLevel, List<int> cost)
    {
        _maxLevel = maxLevel;
        _cost = cost;
    }
}

public struct SavableStatData
{
    public int _currentLevel;

    public SavableStatData(int currentLevel)
    {
        _currentLevel = currentLevel;
    }
}

public class StatSelectPage : MonoBehaviour
{
    [SerializeField] StatInfoViewer _statInfoViewer;
    [SerializeField] Transform _statInfoParent;
    [SerializeField] Button _upgradeBtn;
    [SerializeField] TMP_Text _titleText;

    Dictionary<StatData.Key, Sprite> _statSprite;
    Dictionary<StatData.Key, StatData> _statData;

    BaseFactory _viewerFactory;
    List<BaseViewer> _statViewers;
    StatInfoController _statInfoController;

    StatData.Key _selectedStatKey;
    LobbyTopModel _lobbyTopModel;

    ConfirmationMessageViewer _messageViewer;

    public void Initialize(
        Dictionary<StatData.Key, Sprite> statSprite,
        Dictionary<StatData.Key, StatData> statData,
        BaseFactory viewerFactory,
        ConfirmationMessageViewer messageViewer,
        LobbyTopModel lobbyTopModel)
    {
        _statViewers = new List<BaseViewer>();
        _statSprite = statSprite;
        _statData = statData;
        _viewerFactory = viewerFactory;
        _messageViewer = messageViewer;
        _lobbyTopModel = lobbyTopModel;

        _titleText.text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Stat);

        _upgradeBtn.GetComponentInChildren<TMP_Text>().text = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Upgrade);
        _upgradeBtn.onClick.AddListener(() => { OnClickUpgrade(); });
        _statInfoController = new StatInfoController(new StatInfoModel(_statInfoViewer));

        int count = System.Enum.GetValues(typeof(StatData.Key)).Length;
        for (int i = 0; i < count; i++)
        {
            StatViewer statViewer = (StatViewer)viewerFactory.Create(BaseViewer.Name.StatViewer);
            StatData.Key statType = (StatData.Key)i;

            statViewer.transform.SetParent(_statInfoParent);
            statViewer.transform.localScale = Vector3.one;

            statViewer.Initialize(statSprite[statType], () => { OnClickViewer(statType); });
            _statViewers.Add(statViewer);
        }

        OnClickViewer((StatData.Key)0); // 첫 Stat 선택해주기
    }

    // 돈 읽어와서 빼주기
    void OnClickUpgrade()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터
        int currentStatLevel = saveData._statInfos[_selectedStatKey]._currentLevel;

        StatData statData = _statData[_selectedStatKey];
        int maxStatLevel = statData.MaxLevel;
        int currentCost = statData.GetCost(currentStatLevel);

        bool canUpgrade = saveData._gold >= currentCost;
        if(canUpgrade == false)
        {
            string outOfGold = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.OutOfGold);
            _messageViewer.UpdateInfo(outOfGold);
            _messageViewer.Activate(true);
            return;
        }

        bool nowMaxUpgrade = currentStatLevel == maxStatLevel;
        if (nowMaxUpgrade == true)
        {
            string maximumUpgradeStatus = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.MaximumUpgradeStatus);
            _messageViewer.UpdateInfo(maximumUpgradeStatus);
            _messageViewer.Activate(true);
            return;
        }

        _lobbyTopModel.GoldCount = saveData._gold - currentCost; // 골드 빼주기

        int nextLevel = currentStatLevel + 1; // 레벨 추가
        saveable.ChangeStat(_selectedStatKey, nextLevel); // 스텟을 적용시켜준다.


        string statDescription = ServiceLocater.ReturnLocalizationHandler().GetWord($"{_selectedStatKey.ToString()}Description{nextLevel}");
        _statInfoController.UpdateStat(
            nextLevel,
            statData.GetCost(nextLevel),
            statData.GetDescription(statDescription)
        );
    }

    void OnClickViewer(StatData.Key key)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터
        int currentStatLevel = saveData._statInfos[key]._currentLevel;

        StatData statData = _statData[key];

        string nameDescription = ServiceLocater.ReturnLocalizationHandler().GetWord($"{key.ToString()}Name");
        string statDescription = ServiceLocater.ReturnLocalizationHandler().GetWord($"{key.ToString()}Description{currentStatLevel}");

        _selectedStatKey = key;
        _statInfoController.UpdateStat(
            _statSprite[key],

            nameDescription, //_statData[key].Name,
            currentStatLevel,
            statData.GetCost(currentStatLevel),
            statData.GetDescription(statDescription)
        );
    }
}
