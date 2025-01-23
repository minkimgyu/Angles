using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    LobbyTopModel _lobbyTopModel;

    [SerializeField] LobbyTopViewer _lobbyTopViewer;
    [SerializeField] LobbyScrollController _lobbyScrollController;

    [SerializeField] ConfirmationMessageViewer _popUpViewer;

    [SerializeField] LevelSelectPage _levelSelectPage;

    [SerializeField] DecisionMessageViewer _adViewer;

    [SerializeField] StatSelectPage _statSelectPage;
    [SerializeField] SkinSelectPage _skinSelectPage;

    [SerializeField] Scrollbar _horizontalScrollbar;

    AdHandler _adHandler;

    int _adCoinCount = 100;

    Dictionary<GameMode.Level, LevelData> GetLevelDatas(AddressableHandler addressableHandler, SaveData saveData)
    {
        Dictionary<GameMode.Level, LevelData> levelDatas = new Dictionary<GameMode.Level, LevelData>();

        Dictionary<GameMode.Level, ILevelInfo> info = addressableHandler.Database.LevelDatas;
        Dictionary<GameMode.Level, ISavableLevelInfo> savableInfo = saveData._levelInfos;

        foreach (GameMode.Level level in Enum.GetValues(typeof(GameMode.Level)))
        {
            LevelData levelData = new LevelData(info[level], savableInfo[level], addressableHandler.LevelIconAsset[level]);
            levelDatas.Add(level, levelData);
        }

        return levelDatas;
    }

    private void Update()
    {
        // ���� �� �� �ִ� �ð��̰� ���� �ε�Ǽ� �غ� ������ ���
        if (_adHandler == null) return;
        bool canShowAd = _adHandler.CanShowAdd && ServiceLocater.ReturnAdMobManager().CanShowAdd();

        // ��ư Ȱ��ȭ
        _lobbyTopModel.ActiveAdBtn = canShowAd;
        _lobbyTopModel.AdDuration = _adHandler.LeftTime;
    }

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null) return;

        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // ����� ������

        _lobbyScrollController.Initialize();
        _popUpViewer.Initialize();

        _adCoinCount = addressableHandler.Database.AdData.LobbyAdCoinCount;
        string lobbyAdSaveKeyName = addressableHandler.Database.AdData.LobbyAdSaveKeyName;
        int lobbyAdDelay = addressableHandler.Database.AdData.LobbyAdDelay;
        _adHandler = new AdHandler(lobbyAdSaveKeyName, lobbyAdDelay);

        _adViewer.Initialize
        (
            () =>
            {
                _adViewer.Activate(false);
                ServiceLocater.ReturnAdMobManager().ShowRewardedAd
                (
                    () =>
                    {
                        _adHandler.ResetAdShowTime(); // ���� �� �� �ִٸ� �ʱ�ȭ���ֱ�
                        _lobbyTopModel.GoldCount += _adCoinCount;
                    },
                    () =>
                    {
                        Debug.Log("���� ��� ����");
                    }
                );
            },
            () =>
            {
                _adViewer.Activate(false);
            }
        );

        string state = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.TabToGetGold);
        _adViewer.UpdateInfo(state);

        _lobbyTopViewer.Initialize
        (
            () => 
            {
                _adViewer.Activate(true);
            },
            () =>
            {
                ServiceLocater.ReturnSettingController().Activate(true);
            }
        );

        _lobbyTopModel = new LobbyTopModel(_lobbyTopViewer);
        _lobbyTopModel.GoldCount = saveData._gold;

        LobbyViewerFactory lobbyViewerFactory = new LobbyViewerFactory(addressableHandler.ViewerPrefabAsset);
        Dictionary<GameMode.Level, LevelData> levelDatas = GetLevelDatas(addressableHandler, saveData);

        _levelSelectPage.Initialize(
            levelDatas,
            lobbyViewerFactory
        );

        _statSelectPage.Initialize(
            addressableHandler.StatIconAsset,
            addressableHandler.Database.StatDatas,
            lobbyViewerFactory,
            _popUpViewer,
            _lobbyTopModel
        );

        _skinSelectPage.Initialize(
           addressableHandler.SkinIconAsset,
           addressableHandler.Database.SkinDatas,
           lobbyViewerFactory,
           _popUpViewer,
           _lobbyTopModel
       );
    }
}
