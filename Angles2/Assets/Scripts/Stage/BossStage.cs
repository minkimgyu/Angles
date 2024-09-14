using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : BattleStage
{
    // 적의 지속적 생성
    // 보스 몬스터 + 체력바 생성

    [System.Serializable]
    public struct LevelData
    {
        [SerializeField] TrasformEnemyNameDictionary _levelDictionary;
        public TrasformEnemyNameDictionary LevelDictionary { get { return _levelDictionary; } }
    }

    [SerializeField] Transform _bossSpawnPoint;
    [SerializeField] List<LevelData> _stageDatas;
    Timer _timer;
    float _spawnDelay = 0;

    public override void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection)
    {
        base.Initialize(baseStageController, factoryCollection);
        _timer = new Timer();
        _spawnDelay = 10f;
    }

    protected override void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        GameStateManager.Instance.ChangeEnemyDieCount(1);

        if (_enemyCount > 0) return;
        Debug.Log(_enemyCount);

        _baseStageController.OnStageClearRequested();
    }

    private void Update()
    {
        if (_timer.CurrentState == Timer.State.Finish)
        {
            SpawnMob();

            _timer.Reset();
            _timer.Start(_spawnDelay);
            return;
        }

        _timer.Start(_spawnDelay);
    }

    void SpawnMob()
    {
        int randomIndex = Random.Range(0, _stageDatas.Count);
        LevelData levelData = _stageDatas[randomIndex];

        foreach (var item in levelData.LevelDictionary)
        {
            BaseLife enemy = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(item.Value);
            enemy.transform.position = item.Key.position;
            enemy.AddPathfindEvent(_pathfinder.FindPath);
        }
    }

    // 여기에 보스 생성
    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        BaseLife enemy = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(BaseLife.Name.Lombard);

        enemy.transform.position = _bossSpawnPoint.position;
        enemy.AddObserverEvent(OnEnemyDieRequested);
        enemy.AddPathfindEvent(_pathfinder.FindPath);
        _enemyCount++;
    }
}
