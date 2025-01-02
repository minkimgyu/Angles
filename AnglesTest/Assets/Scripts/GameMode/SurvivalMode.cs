using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SurvivalMode : GameMode
{
    [SerializeField] ArrowPointerController _arrowPointerController;
    [SerializeField] CameraController _cameraController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;

    [SerializeField] SurvivalLevelUIController _levelUIController;

    [SerializeField] CoinViewer _coinViewer;
    [SerializeField] Button _settingBtn;

    DropController _dropController;
    StopwatchTimer _stopwatchTimer;

    ILevel _level;

    List<int> _coinGaugeData;
    int _maxCoinLevel = 0;
    int _currentCoinLevel = 0;
    int _totalCoinCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void OnEnd()
    {
        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount);

        int passedTime = (int)_stopwatchTimer.Duration;
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

        float passedTime = _stopwatchTimer.Duration;
        _stopwatchTimer.Stop();
        _gameResultUIController.OnClearRequested(passedTime, GameStateManager.Instance.ReturnCoin());
    }

    public override void OnGameOverRequested()
    {
        OnEnd();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.ChapterFail);
        float passedTime = _stopwatchTimer.Duration;
        _stopwatchTimer.Stop();
        _gameResultUIController.OnFailRequested(passedTime, GameStateManager.Instance.ReturnCoin());
    }

    private void Update()
    {
        _stopwatchTimer.OnUpdate();

        float passedTime = _stopwatchTimer.Duration;
        _levelUIController.ChangePassedTime((int)passedTime);

        _level.SurvivalStageLevel.Spawn(passedTime);
    }

    protected override void Initialize()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null)
        {
            Debug.Log("addressableHandler 존재하지 않음");
            return;
        }

        _stopwatchTimer = new StopwatchTimer();
        _stopwatchTimer.Start();

        _settingBtn.onClick.AddListener(() => { Debug.Log("Setting"); ServiceLocater.ReturnSettingController().Activate(true); });

        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        _levelUIController.Initialize();

        _coinGaugeData = new List<int>(addressableHandler.Database.CoingaugeData);
        _maxCoinLevel = _coinGaugeData.Count;

        _levelUIController.ChangeNeedCoin(_totalCoinCount, _coinGaugeData[_currentCoinLevel]);

        GameState gameState = new GameState(_coinViewer);
        GameStateManager.Instance.Initialize(gameState, (changeCount) => 
        { 
            _totalCoinCount += changeCount; 
            if(_currentCoinLevel < _maxCoinLevel && _totalCoinCount >= _coinGaugeData[_currentCoinLevel])
            {
                _currentCoinLevel++;
                EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateReusableCard, FindObjectOfType<Player>().GetCaster(), 3, 3);
            }

            _levelUIController.ChangeCoinLevel(_currentCoinLevel);
            _levelUIController.ChangeNeedCoin(_totalCoinCount,_coinGaugeData[_currentCoinLevel]);

            int decreaseAmount = 0;
            if(_currentCoinLevel == 0)
            {
                decreaseAmount = 0;
            }
            else
            {
                decreaseAmount = _coinGaugeData[_currentCoinLevel - 1];
            }


            float ratio = (float)(_totalCoinCount - decreaseAmount) / (_coinGaugeData[_currentCoinLevel] - decreaseAmount);
            _levelUIController.ChangeCoinGauge(ratio);
        });

        BaseFactory stageFactory = new SurvivalStageFactory(addressableHandler.LevelAsset, addressableHandler.LevelDesignAsset);
        InGameFactory inGameFactory = new InGameFactory(addressableHandler, stageFactory);

        BaseFactory viewerFactory = inGameFactory.GetFactory(InGameFactory.Type.Viewer);
        BaseFactory skillFactory = inGameFactory.GetFactory(InGameFactory.Type.Skill);

        _arrowPointerController.Initialize(inGameFactory);
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

        Level level = saveData._selectedLevel[Type.Survival];
        _unlockLevel = addressableHandler.Database.LevelDatas[level].UnlockLevel;
        _canUnlock = addressableHandler.Database.LevelDatas[level].CanUnlockLevel;

        int levelIndex = GameMode.GetLevelIndex(Type.Survival, level);


        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{((GameMode.LevelColor)levelIndex).ToString()}BGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

        ILevelInfo levelInfo = addressableHandler.Database.LevelDatas[level];

        _level = inGameFactory.GetFactory(InGameFactory.Type.Level).Create(level);
        _level.SurvivalStageLevel.transform.position = Vector2.zero;
        _level.SurvivalStageLevel.Initialize(this, addressableHandler, inGameFactory, _levelUIController, _arrowPointerController);
    }
}
