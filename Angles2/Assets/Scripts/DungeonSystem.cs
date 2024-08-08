using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSystem : MonoBehaviour
{
    public struct CommandCollection
    {
        public CommandCollection(
            Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand, // ī�� ���� Ŀ�ǵ�
            Command<int, List<SkillUpgradeData>> CreateCardsCommand, // ī�� ���� Ŀ�ǵ�
            Command<ISkillUser> AddSkillUserCommand, // ī��� �����Ǵ� ��ų ����� �߰� Ŀ�ǵ�
            Command<IFollowable> AddCameraTrackerCommand,  // ī�޶� ���� ��� Ŀ���

            Command<int> ChangeCoin) // ���� ���� Ŀ�ǵ�
        {
            _RecreatableCardsCommand = RecreatableCardsCommand;
            _CreateCardsCommand = CreateCardsCommand;
            _AddSkillUserCommand = AddSkillUserCommand;
            _AddCameraTrackerCommand = AddCameraTrackerCommand;

            _ChangeCoin = ChangeCoin;
        }

        Command<int, int, List<SkillUpgradeData>> _RecreatableCardsCommand; // ����� ī�� Ŀ�ǵ�
        public Command<int, int, List<SkillUpgradeData>> RecreatableCardsCommand { get { return _RecreatableCardsCommand; } }

        Command<int, List<SkillUpgradeData>> _CreateCardsCommand; // ī�� ���� Ŀ�ǵ�
        public Command<int, List<SkillUpgradeData>> CreateCardsCommand { get { return _CreateCardsCommand; } }


        Command<ISkillUser> _AddSkillUserCommand; // ī��� �����Ǵ� ��ų ����� �߰� Ŀ�ǵ�
        public Command<ISkillUser> AddSkillUserCommand { get { return _AddSkillUserCommand; } }


        Command<IFollowable> _AddCameraTrackerCommand; // ī�޶� ���� ��� Ŀ���
        public Command<IFollowable> AddCameraTrackerCommand { get { return _AddCameraTrackerCommand; } }

        Command<int> _ChangeCoin; // ���� ȹ�� Ŀ�ǵ�
        public Command<int> ChangeCoin { get { return _ChangeCoin; } }
    }

    public struct ObserverEventCollection
    {
        public ObserverEventCollection(
            Action OnEnemyDieRequested, // �� ��� ������ �̺�Ʈ
            Action<DropData, Vector3> OnDropRequested, // ��� ������ �̺�Ʈ
            Action<float> OnDachRatioChangeRequested, // �뽬 ������ �̺�Ʈ
            Action<float> OnChargeRatioChangeRequested, // ���� ������ �̺�Ʈ
            Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested, // ��ų ������ �̺�Ʈ
            Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested, // ��ų ������ �̺�Ʈ

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

        Action _OnEnemyDieRequested; // ��� ������ �̺�Ʈ
        public Action OnEnemyDieRequested { get { return _OnEnemyDieRequested; } } // ��� ������ �̺�Ʈ

        Action<DropData, Vector3> _OnDropRequested; // ��� ������ �̺�Ʈ
        public Action<DropData, Vector3> OnDropRequested { get { return _OnDropRequested; } } // ��� ������ �̺�Ʈ

        Action<float> _OnDachRatioChangeRequested; // �뽬 ������ �̺�Ʈ
        public Action<float> OnDachRatioChangeRequested { get { return _OnDachRatioChangeRequested; } } // �뽬 ������ �̺�Ʈ

        Action<float> _OnChargeRatioChangeRequested; // ���� ������ �̺�Ʈ
        public Action<float> OnChargeRatioChangeRequested { get { return _OnChargeRatioChangeRequested; } } // ���� ������ �̺�Ʈ

        Action<BaseSkill.Name, BaseSkill> _OnAddSkillRequested; // �뽬 ������ �̺�Ʈ
        public Action<BaseSkill.Name, BaseSkill> OnAddSkillRequested { get { return _OnAddSkillRequested; } } // �뽬 ������ �̺�Ʈ

        Action<BaseSkill.Name, BaseSkill> _OnRemoveSkillRequested; // �뽬 ������ �̺�Ʈ
        public Action<BaseSkill.Name, BaseSkill> OnRemoveSkillRequested { get { return _OnRemoveSkillRequested; } } // �뽬 ������ �̺�Ʈ


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
        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active }, _addressableHandler.SkillIcons, _factory.Create); // --> �������� Factory���� �ʱ�ȭ�ϰԲ� �����
        _cardUIController.Initialize(_database.CardDatas, _database.UpgradeableSkills, _database.SkillDatas, _addressableHandler.SkillIcons, _factory.Create, ChangeCoin, ReturnCoin); // --> Ŀ��� ������ ����ؼ� �����丵 �غ���
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
