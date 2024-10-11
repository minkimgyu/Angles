using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MobStage : BattleStage
{
    [System.Serializable]
    public struct LevelData
    {
        [SerializeField] Difficulty _difficulty;
        public Difficulty Difficulty { get { return _difficulty; } }

        [SerializeField] TrasformEnemyNameDictionary _levelDictionary;
        public TrasformEnemyNameDictionary LevelDictionary { get { return _levelDictionary; } }
    }

    public enum Difficulty
    {
        Easy,
        Nomal,
        Hard,
    }

    Dictionary<Vector2, Difficulty> _difficultyRangeDictionary;
    [SerializeField] List<LevelData> _stageDatas;

    Portal _portal;

    public override void ActivePortal(Vector2 movePos)
    {
        _portal.Active(movePos);
    }

    public override void Exit()
    {
        base.Exit();
        _portal.Disable();
    }

    public override void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection) 
    {
        base.Initialize(baseStageController, factoryCollection);

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
        _bossCount -= 1;
        if (_bossCount > 0) return;
        Debug.Log(_bossCount);

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

    public override void Spawn(int totalStageCount, int currentStageCount)
    {
        Difficulty difficulty = ReturnDifficultyByProgress((float)currentStageCount / totalStageCount);
        Debug.Log(difficulty);

        List<LevelData> possibleLevelDatas = new List<LevelData>();

        for (int i = 0; i < _stageDatas.Count; i++)
        {
            if (_stageDatas[i].Difficulty != difficulty) continue;

            possibleLevelDatas.Add(_stageDatas[i]);
        }

        // _stageDatas[0]
        LevelData levelData = possibleLevelDatas[UnityEngine.Random.Range(0, possibleLevelDatas.Count)];

        foreach (var item in levelData.LevelDictionary)
        {
            BaseLife enemy = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(item.Value);
            enemy.transform.position = item.Key.position;

            enemy.AddObserverEvent(OnEnemyDieRequested);
            enemy.InitializeFSM(_pathfinder.FindPath);
            _bossCount++;
        }
    }
}