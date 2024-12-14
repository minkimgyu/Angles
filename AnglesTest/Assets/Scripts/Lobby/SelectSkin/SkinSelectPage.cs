using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct SkinData
{
    public enum Key
    {
        Normal,
        BloodEater,
        Guard,
    }

    public string _name;
    public int _cost;
    public string _description;

    public SkinData(string name, int cost, string description)
    {
        _name = name;
        _cost = cost;
        _description = description;
    }
}

public class SkinSelectPage : MonoBehaviour
{
    [SerializeField] SkinInfoViewer _skinInfoViewer;
    [SerializeField] Transform _skinInfoParent;
    [SerializeField] Button _upgradeBtn;

    Dictionary<SkinData.Key, Sprite> _skinSprite;
    Dictionary<SkinData.Key, SkinData> _skinData;
    Dictionary<SkinData.Key, SkinViewer> _skinViewers;

    BaseFactory _viewerFactory;
    SkinInfoController _skinInfoController;

    SkinData.Key _selectedSkinKey;
    Action<PopUpViewer.State> ActivatePopUp;

    LobbyTopModel _lobbyTopModel;

    public void Initialize(
        Dictionary<SkinData.Key, Sprite> skinSprite,
        Dictionary<SkinData.Key, SkinData> skinData,
        Dictionary<SkinInfoModel.State, string> btnInfos,
        BaseFactory viewerFactory,
        Action<PopUpViewer.State> ActivatePopUp,
        LobbyTopModel lobbyTopModel)
    {
        _skinViewers = new Dictionary<SkinData.Key, SkinViewer>();
        _skinSprite = skinSprite;
        _skinData = skinData;
        _viewerFactory = viewerFactory;
        this.ActivatePopUp = ActivatePopUp;
        _lobbyTopModel = lobbyTopModel;

        _upgradeBtn.onClick.AddListener(() => { OnClickBtn(); });
        _skinInfoController = new SkinInfoController(new SkinInfoModel(_skinInfoViewer, btnInfos));

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        int count = System.Enum.GetValues(typeof(SkinData.Key)).Length;
        for (int i = 0; i < count; i++)
        {
            bool isLock = saveData._skinLockInfos[(SkinData.Key)i];

            SkinViewer skinViewer = (SkinViewer)viewerFactory.Create(BaseViewer.Name.SkinViewer);
            SkinData.Key skinType = (SkinData.Key)i;
            skinViewer.transform.SetParent(_skinInfoParent);

            skinViewer.Initialize(skinSprite[skinType], () => { OnClickViewer(skinType); });
            skinViewer.ActivateLockImg(isLock);

            _skinViewers.Add(skinType, skinViewer);
        }

        OnClickViewer(saveData._skin); // 장착한 스킨 보여주기
    }

    void EquipSkin(SkinData.Key key)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        saveable.ChangeSkin(key);

        _skinInfoController.SelectSkin();
    }

    // 스킨 구매 시 스킨 Unlock 해주기
    // Unlock이면 자물쇠 해제
    // 돈 읽어와서 빼주기
    void OnClickBtn()
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        // 이미 구매한 경우
        if (saveData._skinLockInfos[_selectedSkinKey] == false)
        {
            EquipSkin(_selectedSkinKey);
            return;
        }

        SkinData skinData = _skinData[_selectedSkinKey];
        int currentCost = skinData._cost;

        bool canBuy = saveData._gold >= currentCost;
        if (canBuy == false)
        {
            ActivatePopUp?.Invoke(PopUpViewer.State.ShortOfGold);
            return;
        }

        _lobbyTopModel.GoldCount = saveData._gold - currentCost; // 골드 빼주기

        // Unlock으로 만들어주기
        _skinViewers[_selectedSkinKey].ActivateLockImg(false);
        _skinInfoController.BuySkin();

        saveable.UnlockSkin(_selectedSkinKey); // 세이브 기능 추가
    }

    // 구매한 대상 선택 시 장착하기로 텍스트 바꾸기
    void OnClickViewer(SkinData.Key key)
    {
        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        bool isSelected = saveData._skin == key;
        bool isLock = saveData._skinLockInfos[key];
        SkinData statData = _skinData[key];

        int cost = 0;
        if(isLock) cost = _skinData[key]._cost;
        _selectedSkinKey = key;

        _skinInfoController.PickSkin(
            _skinSprite[key],
            isLock,
            isSelected,
            _skinData[key]._name,
            cost,
            _skinData[key]._description
        );
    }
}
