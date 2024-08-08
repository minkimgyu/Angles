using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSystem : MonoBehaviour
{
    public struct CommandCollection
    {
        public CommandCollection(
            Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand, // 카드 생성 커맨드
            Command<int, List<SkillUpgradeData>> CreateCardsCommand, // 카드 생성 커맨드
            Command<ISkillUser> AddSkillUserCommand, // 카드로 생성되는 스킬 사용자 추가 커맨드
            Command<IFollowable> AddCameraTrackerCommand,  // 카메라 추적 등록 커멘드

            Command<int> ChangeCoin) // 코인 변경 커맨드
        {
            _RecreatableCardsCommand = RecreatableCardsCommand;
            _CreateCardsCommand = CreateCardsCommand;
            _AddSkillUserCommand = AddSkillUserCommand;
            _AddCameraTrackerCommand = AddCameraTrackerCommand;

            _ChangeCoin = ChangeCoin;
        }

        Command<int, int, List<SkillUpgradeData>> _RecreatableCardsCommand; // 재생성 카드 커맨드
        public Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand { get { return _RecreatableCardsCommand; } }

        Command<int, List<SkillUpgradeData>> _CreateCardsCommand; // 카드 생성 커맨드
        public Command<int, List<SkillUpgradeData>> CreateCardsCommand { get { return _CreateCardsCommand; } }


        Command<ISkillUser> _AddSkillUserCommand; // 카드로 생성되는 스킬 사용자 추가 커맨드
        public Command<ISkillUser> AddSkillUserCommand { get { return _AddSkillUserCommand; } }


        Command<IFollowable> _AddCameraTrackerCommand; // 카메라 추적 등록 커멘드
        public Command<IFollowable> AddCameraTrackerCommand { get { return _AddCameraTrackerCommand; } }

        Command<int> _ChangeCoin; // 코인 획득 커맨드
        public Command<int> ChangeCoin { get { return _ChangeCoin; } }
    }

    public struct ObserverEventCollection
    {
        public ObserverEventCollection(
            Action OnEnemyDieRequested, // 적 사망 옵저버 이벤트
            Action<DropData, Vector3> OnDropRequested, // 드랍 옵저버 이벤트
            Action<float> OnDachRatioChangeRequested, // 대쉬 옵저버 이벤트
            Action<float> OnChargeRatioChangeRequested, // 차지 옵저버 이벤트
            Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested, // 스킬 옵저버 이벤트
            Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested, // 스킬 옵저버 이벤트

            Action OnGameClearRequested,
            Action OnGameOverRequested
        ) 
        {
            _OnEnemyDieRequested = OnEnemyDieRequested;
            _OnDropRequested = OnDropRequested;

            _OnDachRatioChangeRequested = OnDachRatioChangeRequested;
            _OnChargeRatioChangeRequested = OnChargeRatioChangeRequested;
            _OnAddSkillRequested = OnAddSkillRequested;
            _OnRemoveSkillRequested = OnRemoveSkillRequested;

            _OnGameClearRequested = OnGameClearRequested;
            _OnGameOverRequested = OnGameOverRequested;
        }

        Action _OnEnemyDieRequested; // 드랍 옵저버 이벤트
        public Action OnEnemyDieRequested { get { return _OnEnemyDieRequested; } } // 드랍 옵저버 이벤트

        Action<DropData, Vector3> _OnDropRequested; // 드랍 옵저버 이벤트
        public Action<DropData, Vector3> OnDropRequested { get { return _OnDropRequested; } } // 드랍 옵저버 이벤트

        Action<float> _OnDachRatioChangeRequested; // 대쉬 옵저버 이벤트
        public Action<float> OnDachRatioChangeRequested { get { return _OnDachRatioChangeRequested; } } // 대쉬 옵저버 이벤트

        Action<float> _OnChargeRatioChangeRequested; // 차지 옵저버 이벤트
        public Action<float> OnChargeRatioChangeRequested { get { return _OnChargeRatioChangeRequested; } } // 차지 옵저버 이벤트

        Action<BaseSkill.Name, BaseSkill> _OnAddSkillRequested; // 대쉬 옵저버 이벤트
        public Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested { get { return _OnAddSkillRequested; } } // 대쉬 옵저버 이벤트

        Action<BaseSkill.Name, BaseSkill> _OnRemoveSkillRequested; // 대쉬 옵저버 이벤트
        public Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested { get { return _OnRemoveSkillRequested; } } // 대쉬 옵저버 이벤트


        Action _OnGameClearRequested;
        public Action OnGameClearRequested { get { return _OnGameClearRequested; } }

        Action _OnGameOverRequested;
        public Action OnGameOverRequested { get { return _OnGameOverRequested; } }
    }

    AddressableHandler _addressableHandler;
    IFactory _factory;

    Database _database;
    DropHandler _dropHandler;
    StageController _stageController;

    MapGenerator _mapGenerator;
    [SerializeField] SoundPlayer _soundPlayer;

    [SerializeField] InputController _inputController;
    [SerializeField] CameraController _cameraController;
    [SerializeField] DashUIController _dashUIController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardUIController _cardUIController;

    [SerializeField] BaseViewer _chargeViewer;
    [SerializeField] BaseViewer _coinViewer;
    [SerializeField] BaseViewer _enemyDieCountViewer;

    [SerializeField] BaseViewer _gameEndViewer;

    CommandCollection _commandCollection;
    ObserverEventCollection _eventCollection;

    int _coinCount;
    int CoinCount { get { return _coinCount; } set { _coinCount = value; _coinViewer.UpdateViewer(_coinCount); } }

    int _enemyDieCount;
    int EnemyDieCount { get { return _enemyDieCount; } set { _enemyDieCount = value; _enemyDieCountViewer.UpdateViewer(_enemyDieCount); } }

    // Start is called before the first frame update
    void Start()
    {
        CoinCount = 1000;
        EnemyDieCount = 0;

        _database = new Database();
        _addressableHandler = new AddressableHandler();
        _addressableHandler.Load(Initialize);
    }

    void ChangeEnemyCount() { EnemyDieCount++; }

    void ChangeCoin(int count) { CoinCount += count; }
    int ReturnCoin() { return CoinCount; }

    void OnGameClearRequested()
    {
        _gameEndViewer.TurnOnViewer(true, 0.985f, 1.5f, "Game Clear", Color.white, Color.black);
    }

    void OnGameOverRequested()
    {
        _gameEndViewer.TurnOnViewer(true, 0.985f, 1.5f, "Game Over", Color.black, Color.white);
    }

    private void OnDestroy()
    {
        _addressableHandler.Release();
        _soundPlayer = null;
        ServiceLocater.Provide(_soundPlayer);
    }

    private void InitializeFactory()
    {
        _factory = new FactoryCollection(_addressableHandler, _database);
    }

    private void InitializeUI()
    {
        _dashUIController.Initialize(_factory.Create);
        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active }, _addressableHandler.SkillIcons, _factory.Create); // --> 아이콘을 Factory에서 초기화하게끔 만들기
        _cardUIController.Initialize(_database.CardDatas, _database.UpgradeableSkills, _database.SkillDatas, _addressableHandler.SkillIcons, _factory.Create, ChangeCoin, ReturnCoin); // --> 커멘드 패턴을 사용해서 리팩토링 해보기
        _gameEndViewer.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene("MenuScene"); });
    }

    private void Initialize()
    {
        InitializeFactory();
        InitializeUI();

        _soundPlayer.Initialize(_addressableHandler.AudioAssetDictionary);
        ServiceLocater.Provide(_soundPlayer);
        ServiceLocater.Provide(_inputController);

        _commandCollection = new CommandCollection
        (
            new Command<int, int, List<SkillUpgradeData>>(_cardUIController.CreateCards),
            new Command<int, List<SkillUpgradeData>>(_cardUIController.CreateCards),
            new Command<ISkillUser>(_cardUIController.AddSkillUser),
            new Command<IFollowable>(_cameraController.SetTracker),
            new Command<int>(ChangeCoin)
        );

        _dropHandler = new DropHandler(_database.DropSpreadOffset, _factory, _commandCollection);

        _eventCollection = new ObserverEventCollection
        (
            ChangeEnemyCount,
            _dropHandler.OnDropRequested,
            _dashUIController.UpdateViewer,
            _chargeViewer.UpdateViewer,
            _skillUIController.AddViewer,
            _skillUIController.RemoveViewer,
            OnGameClearRequested,
            OnGameOverRequested
        );

        _cameraController.Initialize();

        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.Initialize(_addressableHandler.StartStageAssetDictionary, _addressableHandler.BonusStageAssetDictionary, _addressableHandler.BattleStageAssetDictionary);
        _mapGenerator.CreateMap();

        _stageController = GetComponent<StageController>();

        StageController.Datas controllerData = new StageController.Datas(30, 5, _factory);
        StageController.Events eventData = new StageController.Events(
            _commandCollection,
            _eventCollection
        );

        _stageController.Initialize(controllerData, eventData);
        _stageController.CreateRandomStage(_mapGenerator.StageObjects);
    }
}
