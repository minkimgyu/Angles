using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class ChapterMode : GameMode
{
    ChapterStageController _stageController;

    [SerializeField] CameraController _cameraController;
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
        ServiceLocater.ReturnSaveManager().ChangeLevelProgress(Type.Chapter, data._selectedLevel[Type.Chapter], stageCount);
    }

    bool _canUnlock;
    Level _unlockLevel;

    void UnlockNextChapter()
    {
        if (_canUnlock == false) return;
        ServiceLocater.ReturnSaveManager().UnlockLevel(Type.Chapter, _unlockLevel);
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
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null)
        {
            Debug.Log("addressableHandler 존재하지 않음");
            return;
        }

        _settingBtn.onClick.AddListener(() => { Debug.Log("Setting"); ServiceLocater.ReturnSettingController().Activate(true); });

        _dungeonStopwatch = new Stopwatch();
        _dungeonStopwatch.Start();

        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState);

        BaseFactory stageFactory = new ChapterStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
        InGameFactory inGameFactory = new InGameFactory(addressableHandler, stageFactory);


        BaseFactory viewerFactory = inGameFactory.GetFactory(InGameFactory.Type.Viewer);
        BaseFactory skillFactory = inGameFactory.GetFactory(InGameFactory.Type.Skill);

        _chargeUIController.Initialize();

        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active },
                                      addressableHandler.SkillIconAsset,
                                      viewerFactory); // --> 아이콘을 Factory에서 초기화하게끔 만들기
        
        _cardUIController.Initialize(addressableHandler.Database.CardDatas, addressableHandler.Database.UpgradeableSkills,
                                     addressableHandler.Database.SkillDatas, addressableHandler.SkillIconAsset,
                                     viewerFactory, skillFactory); // --> 커멘드 패턴을 사용해서 리팩토링 해보기
       
        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.LobbyScene); });

        BaseFactory interactableFactory = inGameFactory.GetFactory(InGameFactory.Type.Interactable);

        // 이거는 StageController로 내려서 사용하자
        // 적이 죽은 경우 사용
        // StartStage에서 플레이어를 등록시키자
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        Level level = saveData._selectedLevel[Type.Chapter];



        _unlockLevel = addressableHandler.Database.LevelDatas[level].UnlockLevel;
        _canUnlock = addressableHandler.Database.LevelDatas[level].CanUnlockLevel;

        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{level.ToString()}BGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

        _stageController = GetComponent<ChapterStageController>();

        ILevelInfo levelInfo = addressableHandler.Database.LevelDatas[level];
        _stageController.Initialize(levelInfo.MaxLevel, addressableHandler, inGameFactory);
        _stageController.CreateRandomStage(level);
    }
}
