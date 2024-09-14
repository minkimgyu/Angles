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

    public override void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection) 
    {
        base.Initialize(baseStageController, factoryCollection);
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

        GameStateManager.Instance.ChangeEnemyDieCount(1);
        if (_enemyCount > 0) return;
        Debug.Log(_enemyCount);

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
        Difficulty difficulty = ReturnDifficultyByProgress((float)totalStageCount / currentStageCount);

        List<LevelData> possibleLevelDatas = new List<LevelData>();

        for (int i = 0; i < _stageDatas.Count; i++)
        {
            if (_stageDatas[i].Difficulty != difficulty) continue;

            possibleLevelDatas.Add(_stageDatas[i]);
        }

        LevelData levelData = _stageDatas[0]; // possibleLevelDatas[UnityEngine.Random.Range(0, possibleLevelDatas.Count)];

        foreach (var item in levelData.LevelDictionary)
        {
            BaseLife enemy = _factoryCollection.ReturnFactory(FactoryCollection.Type.Life).Create(item.Value);
            enemy.transform.position = item.Key.position;

            enemy.AddObserverEvent(OnEnemyDieRequested);
            enemy.AddPathfindEvent(_pathfinder.FindPath);
            _enemyCount++;
        }
    }
}