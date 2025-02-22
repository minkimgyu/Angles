using DamageUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStage : BaseStage, ILevel
{
    Pathfinder _pathfinder;
    SurvivalStageData _survivalStageData;
    int _spawnIndex = 0;

    GameMode _gameMode;

    [SerializeField] Transform _bonusPostion;
    PlayerSpawner _playerSpawner;
    SurvivalLevelUIController _levelUIController;
    ArrowPointerController _arrowPointerController;

    List<BaseLife> _spawnedEnemys;

    public SurvivalStage SurvivalStageLevel { get { return this; } }

    public override void ResetData(SurvivalStageData survivalStageData)
    {
        _survivalStageData = survivalStageData;
    }

    public override void Initialize(
        GameMode gameMode,
        AddressableHandler addressableHandler,
        InGameFactory inGameFactory,
        SurvivalLevelUIController levelUIController,
        ArrowPointerController arrowPointerController)
    {
        base.Initialize(gameMode, addressableHandler, inGameFactory, levelUIController, arrowPointerController);

        _spawnedEnemys = new List<BaseLife>();

        _levelUIController = levelUIController;
        _arrowPointerController = arrowPointerController;

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

    int _spawnCount = 0;
    int _lastSpawnCount = 0;

    public override void Spawn(float passedTime)
    {
        if (_spawnIndex < _survivalStageData.PhaseDatas.Length && _survivalStageData.PhaseDatas[_spawnIndex].SpawnTime < passedTime)
        {
            bool isLastSpawn = _spawnIndex == _survivalStageData.PhaseDatas.Length - 1;
            int size = _survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas.Length;

            if (isLastSpawn)
            {
                _lastSpawnCount = size;

                GameMode.Level level = ServiceLocater.ReturnSaveManager().GetSaveData()._selectedLevel[GameMode.Type.Survival];
                int levelIndex = GameMode.GetLevelIndex(GameMode.Type.Survival, level);

                ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{((GameMode.LevelColor)levelIndex).ToString()}BossBGM");
                ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);

                _levelUIController.ShowStageResult(true);

                string bossIncoming = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.BossIncoming);
                _levelUIController.ChangeStageResultInfo(bossIncoming);
            }

            for (int i = 0; i < size; i++)
            {
                BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(_survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas[i].Name);
                enemy.InjectEvent(() => { _spawnedEnemys.Remove(enemy); });
                _spawnedEnemys.Add(enemy);

                Vector2 spawnPos = _survivalStageData.PhaseDatas[_spawnIndex].SpawnDatas[i].SpawnPosition.V2;

                if (isLastSpawn)
                {
                    TrackableHpViewer hpViewer = (TrackableHpViewer)_inGameFactory.GetFactory(InGameFactory.Type.Viewer).Create(BaseViewer.Name.HpViewer);

                    hpViewer.Initialize();
                    hpViewer.SetFollower(enemy.GetComponent<IFollowable>());

                    enemy.InjectEvent(hpViewer.UpdateRatio);
                    enemy.InjectEvent(OnLastEnemyDie); // 마지막의 경우
                    _arrowPointerController.AddTarget(enemy);
                }
                enemy.transform.position = transform.position + new Vector3(spawnPos.x, spawnPos.y);

                ITrackable trackable = enemy.GetComponent<ITrackable>();
                if (trackable != null)
                {
                    trackable.InjectTarget(_target);
                    trackable.InjectPathfindEvent(_pathfinder.FindPath);
                }
            }

            _spawnIndex++;
        }
    }

    void OnLastEnemyDie()
    {
        _spawnCount++;
        if(_spawnCount == _lastSpawnCount)
        {
            DamageableData damageData = new DamageableData(new DamageStat(DamageUtility.Damage.InstantDeathDamage));
            for (int i = 0; i < _spawnedEnemys.Count; i++)
            {
                _spawnedEnemys[i].GetDamage(damageData);
            }

            _gameMode.OnGameClearRequested();
        }
    }
}
