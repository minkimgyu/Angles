using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TutorialStage : BaseStage, ILevel
{
    public TutorialStage TutorialStageLevel { get { return this; } }

    Pathfinder _pathfinder;
    int _enemyCount;

    GameMode _gameMode;
    PlayerSpawner _playerSpawner;
    TutorialLevelUIController _levelUIController;

    TutorialStageData _tutorialStageData;
    [SerializeField] Transform _skillBoxSpawnPoint;
    [SerializeField] Tilemap _skillBoxWall;

    public override void ResetData(TutorialStageData tutorialStageData)
    {
        _tutorialStageData = tutorialStageData;
    }

    public override void Initialize(
        GameMode gameMode,
        AddressableHandler addressableHandler,
        InGameFactory inGameFactory,
        TutorialLevelUIController levelUIController,
        TutorialMode.Events events)
    {
        base.Initialize(gameMode, addressableHandler, inGameFactory, levelUIController, events);

        _levelUIController = levelUIController;

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

        player.InjectTutorialEvent
        (
            events.MoveStartTutorialEvent,
            events.ShootingTutorialEvent,
            events.CollisionTutorialEvent,
            events.CancelShootingTutorialEvent,
            events.OnGetSkillTutorialEvent
        );

        _target = player;

        IInteractable interactableObject = _inGameFactory.GetFactory(InGameFactory.Type.Interactable).Create(IInteractable.Name.CardTable);
        _spawnedObjects.Add(interactableObject.ReturnGameObject());
        interactableObject.ResetPosition(_skillBoxSpawnPoint.position);

        _gameMode = gameMode;
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);

        OnStageClearTutorialEvent = events.OnStageClearTutorialEvent;

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(() =>{ events.OnEnterPotalTutorialEvent?.Invoke(); });
    }

    Action OnStageClearTutorialEvent;
    Portal _portal;

    public void DestroySkillBoxWall()
    {
        Destroy(_skillBoxWall.gameObject);
    }

    public override void ActivePortal(Vector2 movePos = default)
    {
        _portal.Active(movePos);
    }

    public override void Exit()
    {
        base.Exit();
        _portal.Disable();
    }

    ITarget _target;
    public override void AddPlayer(ITarget target)
    {
        _target = target;
    }

    public override void Spawn() 
    {
        SpawnData[] spawnDatas = _tutorialStageData.SpawnDatas;
        for (int i = 0; i < spawnDatas.Length; i++)
        {
            BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(spawnDatas[i].Name);
            enemy.transform.position = transform.position + new Vector3(spawnDatas[i].SpawnPosition.x, spawnDatas[i].SpawnPosition.y);

            enemy.InjectEvent(OnEnemyDieRequested);
            //enemy.InitializeFSM(_pathfinder.FindPath);

            ITrackable trackable = enemy.GetComponent<ITrackable>();
            if (trackable != null) trackable.InjectTarget(_target);
            _enemyCount++;
        }
    }

    void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        if (_enemyCount > 0) return;
        Debug.Log(_enemyCount);

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.StageClear, 0.8f);
        OnStageClearTutorialEvent?.Invoke();
    }
}
