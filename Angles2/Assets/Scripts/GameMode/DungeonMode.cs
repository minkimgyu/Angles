using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEditor;

public class DungeonMode : BaseGameMode
{
    public enum Chapter
    {
        TriconChapter,
        RhombusChapter,
        PentagonicChapter,
    }

    DungeonStageController _stageController;

    MapGenerator _mapGenerator;
    [SerializeField] CameraController _cameraController;
    [SerializeField] DashUIController _dashUIController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;

    [SerializeField] BaseViewer _coinViewer;
    [SerializeField] Button _settingBtn;

    DropController _dropController;

    Stopwatch _dungeonStopwatch;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void OnEnd()
    {
        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount);

        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        int stageCount = _stageController.ReturnCurrentStageCount();
        ServiceLocater.ReturnSaveManager().ChangeStageProgress(data._chapter, stageCount);
    }

    void UnlockNextChapter()
    {
        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        int lastChapterIndex = Enum.GetValues(typeof(DungeonMode.Chapter)).Length - 1;
        if (lastChapterIndex > (int)saveData._chapter)
        {
            ServiceLocater.ReturnSaveManager().UnlockChapter(saveData._chapter + 1);
        }
    }

    public override void OnGameClearRequested()
    {
        OnEnd();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.ChapterClear);
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.SetPlayerInvincible);
        
        UnlockNextChapter();

        float passedTime = (float)_dungeonStopwatch.Elapsed.TotalSeconds;
        _dungeonStopwatch.Stop();
        _gameResultUIController.OnClearRequested(passedTime, GameStateManager.Instance.ReturnCoin());
    }

    public override void OnGameOverRequested()
    {
        OnEnd();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.ChapterFail);
        float passedTime = (float)_dungeonStopwatch.Elapsed.TotalSeconds;
        _dungeonStopwatch.Stop();
        _gameResultUIController.OnFailRequested(passedTime, GameStateManager.Instance.ReturnCoin());
    }

    protected override void Initialize()
    {
        CoreSystem coreSystem = FindObjectOfType<CoreSystem>();
        if (coreSystem == null)
        {
            Debug.Log("coreSystem 존재하지 않음");
            return;
        }

        _settingBtn.onClick.AddListener(() => { Debug.Log("Setting"); ServiceLocater.ReturnSettingController().Activate(true); });

        _dungeonStopwatch = new Stopwatch();
        _dungeonStopwatch.Start();

        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState);

        BaseFactory viewerFactory = coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer);
        BaseFactory skillFactory = coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Skill);

        _chargeUIController.Initialize();

        _dashUIController.Initialize(coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Viewer));

        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active },
                                      coreSystem.AddressableHandler.SkillIconAsset,
                                      viewerFactory); // --> 아이콘을 Factory에서 초기화하게끔 만들기
        
        _cardUIController.Initialize(coreSystem.Database.CardDatas, coreSystem.Database.UpgradeableSkills,
                                     coreSystem.Database.SkillDatas, coreSystem.AddressableHandler.SkillIconAsset,
                                     viewerFactory, skillFactory); // --> 커멘드 패턴을 사용해서 리팩토링 해보기
       
        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene("LobbyScene"); });

        BaseFactory interactableFactory = coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Interactable);

        // 이거는 StageController로 내려서 사용하자
        // 적이 죽은 경우 사용
        // StartStage에서 플레이어를 등록시키자
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        DungeonMode.Chapter chapter = saveData._chapter;
        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{chapter.ToString()}BGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.Initialize(coreSystem.AddressableHandler.ChapterMapAsset[chapter]);
        _mapGenerator.CreateMap();

        _stageController = GetComponent<DungeonStageController>();

        ChapterInfo chapterInfo = saveData._chapterInfos[chapter];
        _stageController.Initialize(chapterInfo._maxLevel, coreSystem);
        _stageController.CreateRandomStage(_mapGenerator.StageObjects);
    }
}
