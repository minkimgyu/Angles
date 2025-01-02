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


    [SerializeField] StatSelectPage _statSelectPage;
    [SerializeField] SkinSelectPage _skinSelectPage;

    [SerializeField] Scrollbar _horizontalScrollbar;

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

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        _horizontalScrollbar.value = 0; // 스크롤바 초기화

        _lobbyScrollController.Initialize();
        _popUpViewer.Initialize();

        _lobbyTopViewer.Initialize(() => { ServiceLocater.ReturnSettingController().Activate(true); });
        _lobbyTopModel = new LobbyTopModel(_lobbyTopViewer);
        _lobbyTopModel.GoldCount = saveData._gold;

        LobbyViewerFactory lobbyViewerFactory = new LobbyViewerFactory(addressableHandler.ViewerPrefabAsset);
        Dictionary<GameMode.Level, LevelData> typeDatas = GetLevelDatas(addressableHandler, saveData);

        _chapterLevelSelectPage.Initialize(
            typeDatas,
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
