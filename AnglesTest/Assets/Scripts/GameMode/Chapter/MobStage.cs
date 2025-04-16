using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStage : BattleStage
{
    MobStageData _mobStageData;

    public override void ResetData(MobStageData mobStageData)
    {
        _mobStageData = mobStageData;
    }

    public enum Difficulty
    {
        Easy,
        Nomal,
        Hard,
    }

    Dictionary<Vector2, Difficulty> _difficultyRangeDictionary;
    Portal _portal;

    public override void ActivePortal(Vector2 movePos = default)
    {
        _portal.Active(movePos);
    }

    public override void Exit()
    {
        base.Exit();
        _portal.Disable();
    }

    public override void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory) 
    {
        base.Initialize(baseStageController, addressableHandler, inGameFactory);

        _portal = GetComponentInChildren<Portal>();
        _portal.Initialize(_baseStageController.OnMoveToNextStageRequested);

        _difficultyRangeDictionary = new Dictionary<Vector2, Difficulty>
        {
            { new Vector2(0f, 0.3f), Difficulty.Easy},
            { new Vector2(0.3f, 0.7f), Difficulty.Nomal},
            { new Vector2( 0.7f, 1.0f), Difficulty.Hard},
        };
    }

    protected override void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        if (_enemyCount > 0) return;

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.StageClear, 0.8f);
        _baseStageController.OnStageClearRequested();
    }

    Difficulty ReturnDifficultyByProgress(float ratio)
    {
        foreach (var item in _difficultyRangeDictionary)
        {
            if (ratio < item.Key.x || ratio > item.Key.y) continue;

            return item.Value;
        }

        return Difficulty.Nomal;
    }

    ITarget _target;
    public override void AddPlayer(ITarget target)
    {
        _target = target;
    }

    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        Difficulty difficulty = ReturnDifficultyByProgress((float)currentStageCount / totalStageCount);
        Debug.Log(difficulty);

        SpawnData[] spawnDatas = _mobStageData.GetSpawnData(difficulty);
        for (int i = 0; i < spawnDatas.Length; i++)
        {
            BaseLife enemy = _inGameFactory.GetFactory(InGameFactory.Type.Life).Create(spawnDatas[i].Name);
            enemy.transform.position = transform.position + new Vector3(spawnDatas[i].SpawnPosition.x, spawnDatas[i].SpawnPosition.y);

            enemy.InjectEvent(OnEnemyDieRequested);

            ITrackable trackable = enemy.GetComponent<ITrackable>();
            if (trackable != null)
            {
                trackable.InjectTarget(_target);
                trackable.InjectPathfindEvent(_pathfinder.FindPath);
            }

            _enemyCount++;
        }
    }
}