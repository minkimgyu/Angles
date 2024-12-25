using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SurvivalMode : GameMode
{
    [SerializeField] CameraController _cameraController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;

    [SerializeField] BaseViewer _coinViewer;
    [SerializeField] Button _settingBtn;

    DropController _dropController;
    Stopwatch _dungeonStopwatch;

    BaseStage _survivalStage;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void OnEnd()
    {
        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount);

        int passedTime = (int)_dungeonStopwatch.Elapsed.TotalSeconds;
        SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
        ServiceLocater.ReturnSaveManager().ChangeLevelDuration(Type.Chapter, data._selectedLevel[Type.Chapter], passedTime);
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
            Debug.Log("addressableHandler �������� ����");
            return;
        }

        _settingBtn.onClick.AddListener(() => { Debug.Log("Setting"); ServiceLocater.ReturnSettingController().Activate(true); });

        _dungeonStopwatch = new Stopwatch();
        _dungeonStopwatch.Start();

        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState);

        BaseFactory stageFactory = new SurvivalStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
        InGameFactory inGameFactory = new InGameFactory(addressableHandler, stageFactory);


        BaseFactory viewerFactory = inGameFactory.GetFactory(InGameFactory.Type.Viewer);
        BaseFactory skillFactory = inGameFactory.GetFactory(InGameFactory.Type.Skill);

        _chargeUIController.Initialize();

        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active },
                                      addressableHandler.SkillIconAsset,
                                      viewerFactory); // --> �������� Factory���� �ʱ�ȭ�ϰԲ� �����

        _cardUIController.Initialize(addressableHandler.Database.CardDatas, addressableHandler.Database.UpgradeableSkills,
                                     addressableHandler.Database.SkillDatas, addressableHandler.SkillIconAsset,
                                     viewerFactory, skillFactory); // --> Ŀ��� ������ ����ؼ� �����丵 �غ���

        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.LobbyScene); });

        BaseFactory interactableFactory = inGameFactory.GetFactory(InGameFactory.Type.Interactable);

        // �̰Ŵ� StageController�� ������ �������
        // ���� ���� ��� ���
        // StartStage���� �÷��̾ ��Ͻ�Ű��
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        Level level = saveData._selectedLevel[Type.Chapter];
        _unlockLevel = addressableHandler.Database.LevelDatas[level].UnlockLevel;
        _canUnlock = addressableHandler.Database.LevelDatas[level].CanUnlockLevel;

        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{level.ToString()}BGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

        ILevelInfo levelInfo = addressableHandler.Database.LevelDatas[level];
        _survivalStage = inGameFactory.GetFactory(InGameFactory.Type.Stage).Create(level);
        _survivalStage.transform.position = Vector2.zero;

        _survivalStage.Initialize(this, addressableHandler, inGameFactory);
    }
}
