using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStage : BaseStage, ILevel
{
    protected Pathfinder _pathfinder;
    SurvivalStageData _survivalStageData;
    float _passedTime = 0;
    int _spawnIndex = 0;

    GameMode _gameMode;

    public SurvivalStage SurvivalStageLevel { get { return this; } }

    public override void ResetData(SurvivalStageData survivalStageData)
    {
        _survivalStageData = survivalStageData;
    }

    public override void Initialize(GameMode gameMode, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(gameMode, addressableHandler, inGameFactory);

        _gameMode = gameMode;
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);
    }

    public override void Spawn(float passedTime)
    {
        if (_spawnIndex >= _survivalStageData.PhaseDatas.Length) return;

        _passedTime += Time.deltaTime;
        if (_survivalStageData.PhaseDatas[_spawnIndex].SpawnTime > _passedTime)
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
            }
        }
    }

    void OnLastEnemyDie()
    {
        _gameMode.OnGameClearRequested();
    }
}
