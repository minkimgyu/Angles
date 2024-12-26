using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStage : BaseStage, ILevel
{
    protected Pathfinder _pathfinder;
    SurvivalStageData _survivalStageData;
    int _spawnIndex = 0;

    GameMode _gameMode;

    [SerializeField] Transform _bonusPostion;
    PlayerSpawner _playerSpawner;

    public SurvivalStage SurvivalStageLevel { get { return this; } }

    public override void ResetData(SurvivalStageData survivalStageData)
    {
        _survivalStageData = survivalStageData;
    }

    public override void Initialize(GameMode gameMode, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(gameMode, addressableHandler, inGameFactory);

        InputController inputController = FindObjectOfType<InputController>();
        _playerSpawner = new PlayerSpawner(
            inGameFactory,
            inputController,
            addressableHandler.SkinIconAsset,
            addressableHandler.Database.SkinDatas,
            addressableHandler.Database.SkinModifiers,
            addressableHandler.Database.StatDatas,
            addressableHandler.Database.StatModifiers
        );

        Vector3 entryPos = ReturnEntryPosition();
        Player player = _playerSpawner.Spawn();
        player.transform.position = entryPos;

        _target = player;

        IInteractable interactableObject = _inGameFactory.GetFactory(InGameFactory.Type.Interactable).Create(IInteractable.Name.CardTable);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_bonusPostion.position);

        _gameMode = gameMode;
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);
    }

    ITarget _target;

    public override void Spawn(float passedTime)
    {
        if (_spawnIndex >= _survivalStageData.PhaseDatas.Length) return;

        if (_survivalStageData.PhaseDatas[_spawnIndex].SpawnTime < passedTime)
        {
            _spawnIndex++;

            bool isLastSpawn = _spawnIndex == _survivalStageData.PhaseDatas.Length;

            int size = _survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas.Length;
            for (int i = 0; i < size; i++)
            {
                BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(_survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas[i].Name);
                Vector2 spawnPos = _survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas[i].SpawnPosition.V2;

                enemy.transform.position = transform.position + new Vector3(spawnPos.x, spawnPos.y);
                if (isLastSpawn) enemy.AddObserverEvent(OnLastEnemyDie); // 마지막의 경우
                enemy.InitializeFSM(_pathfinder.FindPath);
                enemy.AddTarget(_target);
            }
        }
    }

    void OnLastEnemyDie()
    {
        _gameMode.OnGameClearRequested();
    }
}
