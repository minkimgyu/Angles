using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct StatData
{
    public enum Key
    {
        AttackDamage,
        MoveSpeed,
        MaxHp,
        AutoHpRecovery,
        DamageReduction,
    }

    public string _name;
    List<int> _cost;
    List<string> _description;

    public int _maxLevel;

    public int ReturnCost(int level) 
    { 
        if(level == _maxLevel) return 0;
        return _cost[level]; 
    }

    public string ReturnDescription(int level) 
    {
        if (level == 0) return string.Empty;
        return _description[level - 1]; // �ϳ��� �ռ� ���� ����
    }

    public StatData(string name, List<int> cost, int maxLevel, List<string> description)
    {
        _name = name;
        _cost = cost;
        _maxLevel = maxLevel;
        _description = description;
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
        int currentStatLevel = saveData._statLevelInfos[_selectedStatKey];

        StatData statData = _statData[_selectedStatKey];
        int maxStatLevel = statData._maxLevel;
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
        int currentStatLevel = saveData._statLevelInfos[key];

        StatData statData = _statData[key];

        _selectedStatKey = key;
        _statInfoController.UpdateStat(
            _statSprite[key], 
            _statData[key]._name,
            currentStatLevel,
            statData.ReturnCost(currentStatLevel),
            statData.ReturnDescription(currentStatLevel)
        );
    }
}
