using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BattleStageLevelData
{
    [SerializeField] BattleStage.Difficulty _difficulty;
    public BattleStage.Difficulty Difficulty { get { return _difficulty; } }

    [SerializeField] TrasformEnemyNameDictionary _levelDictionary;
    public TrasformEnemyNameDictionary LevelDictionary { get { return _levelDictionary; } }
}

public class BattleStage : BaseStage
{
    public enum Difficulty
    {
        Easy,
        Nomal,
        Hard,
    }

    Dictionary<Vector2, Difficulty> _difficultyRangeDictionary;

    [SerializeField] List<BattleStageLevelData> _stageDatas;
    int _enemyCount = 0;

    public override void Initialize(System.Action OnClearRequested)
    {
        base.Initialize(OnClearRequested);
        _difficultyRangeDictionary = new Dictionary<Vector2, Difficulty>
        {
            { new Vector2(0f, 0.3f), Difficulty.Easy},
            { new Vector2(0.3f, 0.7f), Difficulty.Easy},
            { new Vector2( 0.7f, 1.0f), Difficulty.Easy},
        };
    }

    void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        if (_enemyCount > 0) return;

        OnClearRequested?.Invoke();
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

    public override void Spawn(StageSpawnData data)
    {
        Difficulty difficulty = ReturnDifficultyByProgress(data.ProgressRatio);

        List<BattleStageLevelData> possibleLevelDatas = new List<BattleStageLevelData>();

        for (int i = 0; i < _stageDatas.Count; i++)
        {
            if (_stageDatas[i].Difficulty != difficulty) continue;

            possibleLevelDatas.Add(_stageDatas[i]);
        }

        BattleStageLevelData levelData = possibleLevelDatas[Random.Range(0, _stageDatas.Count)];

        foreach (var item in levelData.LevelDictionary)
        {
            BaseLife enemy = LifeFactory.Create(item.Value);
            enemy.transform.position = item.Key.position;

            enemy.AddDieEvent(OnEnemyDieRequested);
            _enemyCount++;
        }
    }
}