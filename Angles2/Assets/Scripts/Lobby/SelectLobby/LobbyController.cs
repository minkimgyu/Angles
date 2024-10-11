using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonChapter
{
    TriconChapter,
    RhombusChapter,
    PentagonicChapter,
}

public class LobbyController : MonoBehaviour
{
    [SerializeField] LobbyTopViewer _lobbyTopViewer;
    LobbyTopModel _lobbyTopModel;

    [SerializeField] LevelSelectController _levelSelectController;

    private void Start()
    {
        CoreSystem gameSystem = FindObjectOfType<CoreSystem>();
        ServiceLocater.ReturnSoundPlayer().PlayBGM(ISoundPlayable.SoundName.LobbyBGM);

        ISaveable saveable = ServiceLocater.ReturnSaveManager();
        SaveData saveData = saveable.ReturnSaveData(); // 저장된 데이터

        _lobbyTopModel = new LobbyTopModel(_lobbyTopViewer);
        _lobbyTopModel.GoldCount = saveData._gold;

        _levelSelectController.Initialize(
            saveData._chapter,
            saveData._chapterInfos,
            gameSystem.AddressableHandler.ChapterIconAsset,
            gameSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer)
        );
    }
}
