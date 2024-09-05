using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSystem : BaseGameMode
{
    AddressableHandler _addressableHandler;
    FactoryCollection _factoryCollection;

    Database _database;
    DungeonStageController _stageController;

    MapGenerator _mapGenerator;
    [SerializeField] SoundPlayer _soundPlayer;

    [SerializeField] InputController _inputController;
    [SerializeField] CameraController _cameraController;
    [SerializeField] DashUIController _dashUIController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardUIController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;
    [SerializeField] DungeonGameStateController _dungeonGameStateController; // �� ���� ������ ��Ʈ�ѷ�

    DropController _dropController;

    // Start is called before the first frame update
    void Start()
    {
        _database = new Database();
        _addressableHandler = new AddressableHandler();
        _addressableHandler.Load(Initialize);
    }

    public override void OnGameClearRequested()
    {
        _gameResultUIController.OnClearRequested();
    }

    public override void OnGameOverRequested()
    {
        _gameResultUIController.OnFailRequested();
    }

    private void OnDestroy()
    {
        MainEventBus.Clear();
        SubEventBus.Clear();
        ObserverEventBus.Clear();
        GameStateEventBus.Clear();

        _addressableHandler.Release();
        _soundPlayer = null;
        ServiceLocater.Provide(_soundPlayer);
    }

    private void InitializeFactory()
    {
        _factoryCollection = new FactoryCollection(_addressableHandler, _database);
    }

    private void InitializeUI()
    {
        BaseFactory viewerFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer);
        BaseFactory skillFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Skill);

        _chargeUIController.Initialize();
        _dungeonGameStateController.Initialize();
        _dashUIController.Initialize(_factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer));
        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active }, _addressableHandler.SkillIcons, viewerFactory); // --> �������� Factory���� �ʱ�ȭ�ϰԲ� �����
        _cardUIController.Initialize(_database.CardDatas, _database.UpgradeableSkills, _database.SkillDatas, _addressableHandler.SkillIcons, viewerFactory, skillFactory); // --> Ŀ��� ������ ����ؼ� �����丵 �غ���
        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene("MenuScene"); });
    }

    protected override void Initialize()
    {
        MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        InitializeFactory();
        InitializeUI();

        _soundPlayer.Initialize(_addressableHandler.AudioAssetDictionary);
        ServiceLocater.Provide(_soundPlayer);
        ServiceLocater.Provide(_inputController);

        BaseFactory interactableFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Interactable);

        // �̰Ŵ� StageController�� ������ �������
        // ���� ���� ��� ���
        // StartStage���� �÷��̾ ��Ͻ�Ű��
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.Initialize(_addressableHandler.StartStageAssetDictionary, _addressableHandler.BonusStageAssetDictionary, _addressableHandler.BattleStageAssetDictionary);
        _mapGenerator.CreateMap();

        _stageController = GetComponent<DungeonStageController>();
        _stageController.Initialize(30, 5, _factoryCollection);
        _stageController.CreateRandomStage(_mapGenerator.StageObjects);
    }
}
