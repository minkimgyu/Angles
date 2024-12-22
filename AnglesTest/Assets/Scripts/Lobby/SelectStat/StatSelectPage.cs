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
        //AutoHpRecovery,
        DamageReduction,
    }

    [JsonProperty] string _name;
    [JsonProperty] int _maxLevel;

    [JsonProperty] List<int> _cost;
    [JsonProperty] List<string> _description;

    [JsonIgnore] public string Name { get => _name; }
    [JsonIgnore] public int MaxLevel { get => _maxLevel; }

    public int ReturnCost(int level) 
    { 
        if(level == MaxLevel) return 0;
        return _cost[level]; 
    }

    public string ReturnDescription(int level) 
    {
        if (level == 0) return string.Empty;
        return _description[level - 1]; // �ϳ��� �ռ� ���� ����
    }

    public StatData(string name, int maxLevel, List<int> cost, List<string> description)
    {
        _name = name;
        _maxLevel = maxLevel;
        _cost = cost;
        _description = description;
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

    Dictionary<StatData.Key, Sprite> _statSprite;
    Dictionary<StatData.Key, StatData> _statData;

    BaseFactory _viewerFactory;
    List<BaseViewer> _statViewers;
    StatInfoController _statInfoController;

    StatData.Key _selectedStatKey;
    Action<PopUpViewer.State> ActivatePopUp;

    LobbyTopModel _lobbyTopModel;

    public void Initialize(
        Dictionary<StatData.Key, Sprite> statSprite,
        Dictionary<StatData.Key, StatData> statData,
        BaseFactory viewerFactory,
        Action<PopUpViewer.State> ActivatePopUp,
        LobbyTopModel lobbyTopModel)
    {
        _statViewers = new List<BaseViewer>();
        _statSprite = statSprite;
        _statData = statData;
        _viewerFactory = viewerFactory;
        this.ActivatePopUp = ActivatePopUp;
        _lobbyTopModel = lobbyTopModel;

        _upgradeBtn.onClick.AddListener(() => { OnClickUpgrade(); });
        _statInfoController = new StatInfoController(new StatInfoModel(_statInfoViewer));

        int count = System.Enum.GetValues(typeof(StatData.Key)).Length;
        for (int i = 0; i < count; i++)
        {
            StatViewer statViewer = (StatViewer)viewerFactory.Create(BaseViewer.Name.StatViewer);
            StatData.Key statType = (StatData.Key)i;
            statViewer.transform.SetParent(_statInfoParent);

            statViewer.Initialize(statSprite[statType], () => { OnClickViewer(statType); });
            _statViewers.Add(statViewer);
        }

        OnClickViewer((StatData.Key)0); // ù Stat �������ֱ�
    }

    // �� �о�ͼ� ���ֱ�
    void OnClickUpgrade()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������
        int currentStatLevel = saveData._statLevelInfos[_selectedStatKey]._currentLevel;

        StatData statData = _statData[_selectedStatKey];
        int maxStatLevel = statData.MaxLevel;
        int currentCost = statData.ReturnCost(currentStatLevel);

        bool canUpgrade = saveData._gold >= currentCost;
        if(canUpgrade == false)
        {
            ActivatePopUp?.Invoke(PopUpViewer.State.ShortOfGold);
            return;
        }

        bool nowMaxUpgrade = currentStatLevel == maxStatLevel;
        if (nowMaxUpgrade == true)
        {
            ActivatePopUp?.Invoke(PopUpViewer.State.NowMaxUpgrade);
            return;
        }

        _lobbyTopModel.GoldCount = saveData._gold - currentCost; // ��� ���ֱ�

        int nextLevel = currentStatLevel + 1; // ���� �߰�
        saveable.ChangeStat(_selectedStatKey, nextLevel); // ������ ��������ش�.

        _statInfoController.UpdateStat(
            nextLevel,
            statData.ReturnCost(nextLevel),
            statData.ReturnDescription(nextLevel)
        );
    }

    void OnClickViewer(StatData.Key key)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������
        int currentStatLevel = saveData._statLevelInfos[key]._currentLevel;

        StatData statData = _statData[key];

        _selectedStatKey = key;
        _statInfoController.UpdateStat(
            _statSprite[key], 
            _statData[key].Name,
            currentStatLevel,
            statData.ReturnCost(currentStatLevel),
            statData.ReturnDescription(currentStatLevel)
        );
    }
}
