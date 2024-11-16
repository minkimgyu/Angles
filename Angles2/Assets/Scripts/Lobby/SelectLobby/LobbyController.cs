using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    LobbyTopModel _lobbyTopModel;

    [SerializeField] LobbyTopViewer _lobbyTopViewer;
    [SerializeField] LobbyScrollController _lobbyScrollController;

    [SerializeField] PopUpViewer _popUpViewer;

    [SerializeField] LevelSelectPage _levelSelectPage;
    [SerializeField] StatSelectPage _statSelectPage;
    [SerializeField] SkinSelectPage _skinSelectPage;

    private void Start()
    {
        CoreSystem gameSystem = FindObjectOfType<CoreSystem>();
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.GetSaveData(); // 저장된 데이터

        _lobbyScrollController.Initialize();

        _popUpViewer.Initialize(gameSystem.Database.AlarmInfos);

        _lobbyTopViewer.Initialize(() => { ServiceLocater.ReturnSettingController().Activate(true); });
        _lobbyTopModel = new LobbyTopModel(_lobbyTopViewer);
        _lobbyTopModel.GoldCount = saveData._gold;

        _levelSelectPage.Initialize(
            saveData._chapter,
            saveData._chapterInfos,
            gameSystem.AddressableHandler.ChapterIconAsset,
            gameSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer)
        );

        _statSelectPage.Initialize(
            gameSystem.AddressableHandler.StatIconAsset,
            gameSystem.Database.StatDatas,
            gameSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer),
            _popUpViewer.UpdateInfo,
            _lobbyTopModel
        );

        _skinSelectPage.Initialize(
           gameSystem.AddressableHandler.SkinIconAsset,
           gameSystem.Database.SkinDatas,
           gameSystem.Database.BtnInfos,
           gameSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer),
           _popUpViewer.UpdateInfo,
           _lobbyTopModel
       );
    }
}
