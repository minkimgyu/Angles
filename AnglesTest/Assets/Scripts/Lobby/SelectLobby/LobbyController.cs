using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    LobbyTopModel _lobbyTopModel;

    [SerializeField] LobbyTopViewer _lobbyTopViewer;
    [SerializeField] LobbyScrollController _lobbyScrollController;

    [SerializeField] PopUpViewer _popUpViewer;

    [SerializeField] LevelSelectPage _chapterLevelSelectPage;

    [SerializeField] AdViewer _adViewer;

    [SerializeField] StatSelectPage _statSelectPage;
    [SerializeField] SkinSelectPage _skinSelectPage;

    [SerializeField] Scrollbar _horizontalScrollbar;

    const int _adCoinCount = 100;

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
        bool canLoadAd = ServiceLocater.ReturnAdMobManager().CanLoadAd;
        if (canLoadAd == true)
        {
            ServiceLocater.ReturnAdMobManager().ShowRewardedAd
            (
                () =>
                {
                    Debug.Log("광고 보기 실패");
                }
            );
            ServiceLocater.ReturnAdMobManager().GetAd(); // 광고 로드 완료하면 작동
        }

        bool canGetReward = ServiceLocater.ReturnAdMobManager().CanGetReward;
        if (canGetReward == true)
        {
            _lobbyTopModel.GoldCount += _adCoinCount;
            ServiceLocater.ReturnAdMobManager().GetReward(); // 보상을 받으면 작동
        }
    }

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        _horizontalScrollbar.value = 0; // 스크롤바 초기화

        _lobbyScrollController.Initialize();
        _popUpViewer.Initialize();

        _adViewer.Initialize
        (
            () =>
            {
                _adViewer.TurnOnViewer(false);
                ServiceLocater.ReturnAdMobManager().LoadRewardedAd
                (
                    () =>
                    {
                        Debug.Log("광고 로드 실패");
                    }
                );
            },
            () =>
            {
                _adViewer.TurnOnViewer(false);
            },
            ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.TabToGetGold)
        );

        _lobbyTopViewer.Initialize
        (
            () => 
            {
                _adViewer.TurnOnViewer(true);
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

        _chapterLevelSelectPage.Initialize(
            levelDatas,
            lobbyViewerFactory
        );

        _statSelectPage.Initialize(
            addressableHandler.StatIconAsset,
            addressableHandler.Database.StatDatas,
            lobbyViewerFactory,
            _popUpViewer.UpdateInfo,
            _lobbyTopModel
        );

        _skinSelectPage.Initialize(
           addressableHandler.SkinIconAsset,
           addressableHandler.Database.SkinDatas,
           lobbyViewerFactory,
           _popUpViewer.UpdateInfo,
           _lobbyTopModel
       );
    }
}
