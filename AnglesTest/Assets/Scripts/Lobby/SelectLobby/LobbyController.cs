using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    LobbyTopModel _lobbyTopModel;

    [SerializeField] LobbyTopViewer _lobbyTopViewer;
    [SerializeField] LobbyScrollController _lobbyScrollController;

    [SerializeField] PopUpViewer _popUpViewer;

    [SerializeField] LevelSelectPage _chapterLevelSelectPage;


    [SerializeField] StatSelectPage _statSelectPage;
    [SerializeField] SkinSelectPage _skinSelectPage;

    Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> GetTypeDatas(AddressableHandler addressableHandler, SaveData saveData)
    {
        Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> typeDatas = new Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>>();

        foreach (GameMode.Type type in Enum.GetValues(typeof(GameMode.Type)))
        {
            Dictionary<GameMode.Level, LevelData> levelDatas = new Dictionary<GameMode.Level, LevelData>();

            Dictionary<GameMode.Level, ILevelInfo> info = addressableHandler.Database.LevelDatas[type];
            Dictionary<GameMode.Level, ISavableLevelInfo> savableInfo = saveData._levelTypeInfos[type];
            Dictionary<GameMode.Level, Sprite> levelSprite = addressableHandler.LevelIconAsset[type];

            foreach (KeyValuePair<GameMode.Level, ILevelInfo> item1 in info)
            {
                LevelData levelData = new LevelData(info[item1.Key], savableInfo[item1.Key], levelSprite[item1.Key]);
                levelDatas.Add(item1.Key, levelData);
            }

            typeDatas.Add(type, levelDatas);
        }

        return typeDatas;
    }

    private void Start()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        _lobbyScrollController.Initialize();

        _popUpViewer.Initialize(addressableHandler.Database.PopUpInfos);

        _lobbyTopViewer.Initialize(() => { ServiceLocater.ReturnSettingController().Activate(true); });
        _lobbyTopModel = new LobbyTopModel(_lobbyTopViewer);
        _lobbyTopModel.GoldCount = saveData._gold;

        LobbyViewerFactory lobbyViewerFactory = new LobbyViewerFactory(addressableHandler.ViewerPrefabAsset);
        Dictionary<GameMode.Type, Dictionary<GameMode.Level, LevelData>> typeDatas = GetTypeDatas(addressableHandler, saveData);

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
           addressableHandler.Database.BuyInfos,
           lobbyViewerFactory,
           _popUpViewer.UpdateInfo,
           _lobbyTopModel
       );
    }
}
