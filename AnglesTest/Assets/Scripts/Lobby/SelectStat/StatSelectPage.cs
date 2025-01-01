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

    const string replaceString = "(R)";

    [JsonProperty] List<int> _cost;
    [JsonIgnore] public int MaxLevel { get => _maxLevel; }

    public int GetCost(int level) 
    { 
        if(level == MaxLevel) return 0;
        return _cost[level]; 
    }

    public string GetDescription(int level, float ratio, string txt) 
    {
        txt = txt.Replace(replaceString, (ratio * 100).ToString());

        if (level == 0) return string.Empty;
        return txt; // �ϳ��� �ռ� ���� ����
    }

    public string GetDescription(int level, int value, string txt)
    {
        txt = txt.Replace(replaceString, value.ToString());

        if (level == 0) return string.Empty;
        return txt; // �ϳ��� �ռ� ���� ����
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
        int currentStatLevel = saveData._statInfos[_selectedStatKey]._currentLevel;

        StatData statData = _statData[_selectedStatKey];
        int maxStatLevel = statData.MaxLevel;
        int currentCost = statData.GetCost(currentStatLevel);

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


        //_statInfoController.UpdateStat(
        //    nextLevel,
        //    statData.GetCost(nextLevel),
        //    statData.GetDescription(nextLevel)
        //);
    }

    void OnClickViewer(StatData.Key key)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������
        int currentStatLevel = saveData._statInfos[key]._currentLevel;

        StatData statData = _statData[key];

        _selectedStatKey = key;
        //_statInfoController.UpdateStat(
        //    _statSprite[key], 
        //    _statData[key].Name,
        //    currentStatLevel,
        //    statData.GetCost(currentStatLevel),
        //    statData.GetDescription(currentStatLevel)
        //);
    }
}
