using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class BossStage : BattleStage
{
    List<IDamageable> _mobs;

    bool _isActive = false;
    Timer _timer;
    float _spawnDelay = 0;

    int _spawnCount = 0;
    const int _maxSpawnCount = 7;

    System.Action<float> OnHPChange;

    public override void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(baseStageController, addressableHandler, inGameFactory);

        _mobs = new List<IDamageable>();
        _timer = new Timer();
        _spawnDelay = 15f;
    }

    BossStageData _bossStageData;

    public override void ResetData(BossStageData bossStageData)
    {
        _bossStageData = bossStageData;
    }

    public override void AddBossHPEvent(System.Action<float> OnHPChange)
    {
        this.OnHPChange = OnHPChange;
    }

    protected override void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        if (_enemyCount > 0) return;

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
        if (_timer.CurrentState != Timer.State.Running)
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
        for (int i = 0; i < _bossStageData.MobSpawnDatas.Length; i++)
        {
            if (_spawnCount >= _maxSpawnCount) return;

            BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(_bossStageData.MobSpawnDatas[i].Name);
            _spawnCount++;

            enemy.transform.position = transform.position + new Vector3(_bossStageData.MobSpawnDatas[i].SpawnPosition.x, _bossStageData.MobSpawnDatas[i].SpawnPosition.y);
            enemy.InitializeFSM(_pathfinder.FindPath);
            enemy.AddObserverEvent(() => { _spawnCount--; });

            _mobs.Add(enemy);
        }
    }

    // ���⿡ ���� ����
    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        _isActive = true;
        BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(_bossStageData.BossSpawnData.Name);

        enemy.AddObserverEvent(OnHPChange);
        enemy.AddObserverEvent(OnEnemyDieRequested);
        enemy.InitializeFSM(_pathfinder.FindPath);
        enemy.transform.position = transform.position + new Vector3(_bossStageData.BossSpawnData.SpawnPosition.x, _bossStageData.BossSpawnData.SpawnPosition.y);
        _enemyCount++;

        GameMode.Level level = ServiceLocater.ReturnSaveManager().GetSaveData()._selectedLevel[GameMode.Type.Chapter];
        ISoundPlayable.SoundName bgm = (ISoundPlayable.SoundName)Enum.Parse(typeof(ISoundPlayable.SoundName), $"{level.ToString()}BossBGM");
        ServiceLocater.ReturnSoundPlayer().PlayBGM(bgm);
    }
}
