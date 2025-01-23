using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

abstract public class DungeonMode : GameMode
{
    [SerializeField] CameraController _cameraController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;

    [SerializeField] protected CoinViewer _coinViewer;
    [SerializeField] DecisionMessageViewer _reviveViewer;
    [SerializeField] Button _settingBtn;

    DropController _dropController;
    protected StopwatchTimer _stopwatchTimer;

    bool _canUnlock;
    Level _unlockLevel;

    AdHandler _adHandler;

    void UnlockNextChapter()
    {
        if (_canUnlock == false) return;
        ServiceLocater.ReturnSaveManager().UnlockLevel(_unlockLevel);
    }

    protected virtual void Update()
    {
        _stopwatchTimer.OnUpdate();
    }

    bool _gameEnd;

    protected virtual void OnGameEnd()
    {
        _gameEnd = true;
        _stopwatchTimer.Stop();

        int coinCount = GameStateManager.Instance.ReturnCoin();
        ServiceLocater.ReturnSaveManager().AddCoinCount(coinCount); 
    }

    public override void OnGameClearRequested()
    {
        if (_gameEnd == true) return; // 이미 게임 끝 판정이 난 경우 return
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.SetInvincible);

        OnGameEnd();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.LevelClear);
        UnlockNextChapter();

        float passedTime = _stopwatchTimer.Duration;
        _gameResultUIController.OnClearRequested(passedTime, GameStateManager.Instance.ReturnCoin());
    }

    public override void OnGameOverRequested()
    {
        if (_gameEnd == true) return; // 이미 게임 끝 판정이 난 경우 return

        if (_adHandler.CanShowAdd == false)
        {
            Debug.Log($"광고까지 남은 시간: {_adHandler.LeftTime}");
        }

        // 광고를 볼 수 있다면
        if (CanRevive == true && _adHandler.CanShowAdd == true && ServiceLocater.ReturnAdMobManager().CanShowAdd() == true)
        {
            _reviveViewer.Activate(true);
            _reviveChance -= 1;
        }
        else
        {
            OnGameEnd();
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.LevelFail);

            float passedTime = _stopwatchTimer.Duration;
            _gameResultUIController.OnFailRequested(passedTime, GameStateManager.Instance.ReturnCoin());
        }
    }

    protected override void Initialize(GameMode.Type type)
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null)
        {
            Debug.Log("addressableHandler 존재하지 않음");
            return;
        }

        _gameEnd = false;

        string inGameAdSaveKeyName = addressableHandler.Database.AdData.InGameAdSaveKeyName;
        int inGameAdDelay = addressableHandler.Database.AdData.InGameAdDelay;
        _adHandler = new AdHandler(inGameAdSaveKeyName, inGameAdDelay);

        _settingBtn.onClick.AddListener(() => { Debug.Log("Setting"); ServiceLocater.ReturnSettingController().Activate(true); });

        _stopwatchTimer = new StopwatchTimer();
        _stopwatchTimer.Start();

        _reviveViewer.Initialize
        (
            () =>
            {
                _reviveViewer.Activate(false);
                ServiceLocater.ReturnAdMobManager().ShowRewardedAd
                (
                    () =>
                    {
                        _adHandler.ResetAdShowTime(); // 광고를 본다면 초기화해주기
                        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.Revive);
                    },
                    () =>
                    {
                        _reviveViewer.Activate(false);
                        OnGameOverRequested();
                    }
                );
            },
            () => 
            { 
                _reviveViewer.Activate(false); 
                OnGameOverRequested(); 
            }
        );

        string state = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.TabToRevive);
        _reviveViewer.UpdateInfo(state);

        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        InitializeGameStageManager();

        BaseFactory stageFactory = ReturnStageFactory(addressableHandler);
        InGameFactory inGameFactory = new InGameFactory(addressableHandler, stageFactory);

        BaseFactory viewerFactory = inGameFactory.GetFactory(InGameFactory.Type.Viewer);
        BaseFactory skillFactory = inGameFactory.GetFactory(InGameFactory.Type.Skill);

        _chargeUIController.Initialize();

        _skillUIController.Initialize
        (
            new List<BaseSkill.Type> { BaseSkill.Type.Active },
            addressableHandler.SkillIconAsset,
            viewerFactory
        ); // --> 아이콘을 Factory에서 초기화하게끔 만들기

        _cardUIController.Initialize
        (
            addressableHandler.Database.CardDatas,
            addressableHandler.Database.UpgradeableSkills,
            addressableHandler.Database.SkillDatas,
            addressableHandler.SkillIconAsset,
            viewerFactory,
            skillFactory
        ); // --> 커멘드 패턴을 사용해서 리팩토링 해보기

        _gameResultUIController.Initialize(() =>
        {
            ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.LobbyScene);
        });

        BaseFactory interactableFactory = inGameFactory.GetFactory(InGameFactory.Type.Interactable);

        // 이거는 StageController로 내려서 사용하자
        // 적이 죽은 경우 사용
        // StartStage에서 플레이어를 등록시키자
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        Level level = saveData._selectedLevel[type];
        _unlockLevel = addressableHandler.Database.LevelDatas[level].UnlockLevel;
        _canUnlock = addressableHandler.Database.LevelDatas[level].CanUnlockLevel;

        int levelIndex = GameMode.GetLevelIndex(type, level);

        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{((GameMode.LevelColor)levelIndex).ToString()}BGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

        InitializeLevel(addressableHandler, inGameFactory, level);
    }

    protected abstract void InitializeLevel(AddressableHandler addressableHandler, InGameFactory inGameFactory, Level level);
    protected abstract BaseFactory ReturnStageFactory(AddressableHandler addressableHandler);
    protected abstract void InitializeGameStageManager();
}
