using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSystem : BaseGameMode
{
    FactoryCollection _factoryCollection;

    Database _database;
    DungeonStageController _stageController;

    MapGenerator _mapGenerator;
    [SerializeField] CameraController _cameraController;
    [SerializeField] DashUIController _dashUIController;
    [SerializeField] SkillUIController _skillUIController;
    [SerializeField] CardUIController _cardUIController;

    [SerializeField] GameResultUIController _gameResultUIController;
    [SerializeField] ChargeUIController _chargeUIController;

    [SerializeField] BaseViewer _coinViewer;
    [SerializeField] BaseViewer _enemyDieCountViewer;

    DropController _dropController;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void OnGameClearRequested()
    {
        _gameResultUIController.OnClearRequested();
    }

    public override void OnGameOverRequested()
    {
        _gameResultUIController.OnFailRequested();
    }

    protected override void Initialize()
    {
        AddressableHandler addressableHandler = FindObjectOfType<AddressableHandler>();
        if (addressableHandler == null)
        {
            Debug.Log("AddressableHandler �������� ����");
            return;
        }


        _database = new Database();
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameClear, new GameEndCommand(OnGameClearRequested));
        EventBusManager.Instance.MainEventBus.Register(MainEventBus.State.GameOver, new GameEndCommand(OnGameOverRequested));

        GameState gameState = new GameState(_coinViewer, _enemyDieCountViewer);
        GameStateManager.Instance.Initialize(gameState);

        _factoryCollection = new FactoryCollection(addressableHandler, _database);

        BaseFactory viewerFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer);
        BaseFactory skillFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Skill);

        _chargeUIController.Initialize();
        _dashUIController.Initialize(_factoryCollection.ReturnFactory(FactoryCollection.Type.Viewer));
        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active }, addressableHandler.SkillIcons, viewerFactory); // --> �������� Factory���� �ʱ�ȭ�ϰԲ� �����
        _cardUIController.Initialize(_database.CardDatas, _database.UpgradeableSkills, _database.SkillDatas, addressableHandler.SkillIcons, viewerFactory, skillFactory); // --> Ŀ��� ������ ����ؼ� �����丵 �غ���
        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene("MenuScene"); });

        BaseFactory interactableFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Interactable);

        // �̰Ŵ� StageController�� ������ �������
        // ���� ���� ��� ���
        // StartStage���� �÷��̾ ��Ͻ�Ű��
        _dropController = new DropController(interactableFactory);
        _cameraController.Initialize();

        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.Initialize(addressableHandler.StartStageAssetDictionary, addressableHandler.BonusStageAssetDictionary, addressableHandler.BattleStageAssetDictionary);
        _mapGenerator.CreateMap();

        _stageController = GetComponent<DungeonStageController>();
        _stageController.Initialize(30, 5, _factoryCollection);
        _stageController.CreateRandomStage(_mapGenerator.StageObjects);
    }
}
