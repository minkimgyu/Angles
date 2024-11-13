using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DamageUtility;

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
    [SerializeField] BaseLife.Name _bossName;

    List<IDamageable> _mobs;

    bool _isActive = false;
    Timer _timer;
    float _spawnDelay = 0;

    int _spawnCount = 0;
    const int _maxSpawnCount = 7;

    System.Action<float> OnHPChange;

    public override void Initialize(BaseStageController baseStageController, CoreSystem coreSystem)
    {
        base.Initialize(baseStageController, coreSystem);

        _mobs = new List<IDamageable>();
        _timer = new Timer();
        _spawnDelay = 15f;
    }

    public override void AddBossHPEvent(System.Action<float> OnHPChange)
    {
        this.OnHPChange = OnHPChange;
    }

    protected override void OnEnemyDieRequested()
    {
        _bossCount -= 1;
        if (_bossCount > 0) return;

        _isActive = false;

        DamageableData damageData = new DamageableData(new DamageStat(DamageUtility.Damage.InstantDeathDamage));
        for (int i = 0; i < _mobs.Count; i++)
        {
            Damage.Hit(damageData, _mobs[i]);
        }

        _baseStageController.OnStageClearRequested();
    }

    private void Update()
    {
        if(_isActive == false) return;
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
            if (_spawnCount >= _maxSpawnCount) return;

            BaseLife enemy = _coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(item.Value);
            _spawnCount++;

            enemy.transform.position = item.Key.position;
            enemy.InitializeFSM(_pathfinder.FindPath);
            enemy.AddObserverEvent(() => { _spawnCount--; });

            _mobs.Add(enemy);
        }
    }

    // 여기에 보스 생성
    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        _isActive = true;
        BaseLife enemy = _coreSystem.FactoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(_bossName);

        enemy.AddObserverEvent(OnHPChange);
        enemy.AddObserverEvent(OnEnemyDieRequested);
        enemy.InitializeFSM(_pathfinder.FindPath);
        enemy.transform.position = _bossSpawnPoint.position;
        _bossCount++;

        DungeonMode.Chapter chapter = ServiceLocater.ReturnSaveManager().GetSaveData()._chapter;
        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{chapter.ToString()}BossBGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);
    }
}
