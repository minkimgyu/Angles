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
            Debug.Log("AddressableHandler 존재하지 않음");
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
        _skillUIController.Initialize(new List<BaseSkill.Type> { BaseSkill.Type.Active }, addressableHandler.SkillIcons, viewerFactory); // --> 아이콘을 Factory에서 초기화하게끔 만들기
        _cardUIController.Initialize(_database.CardDatas, _database.UpgradeableSkills, _database.SkillDatas, addressableHandler.SkillIcons, viewerFactory, skillFactory); // --> 커멘드 패턴을 사용해서 리팩토링 해보기
        _gameResultUIController.Initialize(() => { ServiceLocater.ReturnSceneController().ChangeScene("MenuScene"); });

        BaseFactory interactableFactory = _factoryCollection.ReturnFactory(FactoryCollection.Type.Interactable);

        // 이거는 StageController로 내려서 사용하자
        // 적이 죽은 경우 사용
        // StartStage에서 플레이어를 등록시키자
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
