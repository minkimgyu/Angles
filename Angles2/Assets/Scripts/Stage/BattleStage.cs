using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStage : BaseStage
{
    [SerializeField] List<Transform> _enemyPostions;
    int _enemyCount = 0;

    void OnEnemyDieRequested()
    {
        _enemyCount -= 1;
        if (_enemyCount > 0) return;

        OnClearRequested?.Invoke();
    }

    public override void Spawn(List<BaseLife.Name> names)
    {
        int randomRange = Random.Range(0, _enemyPostions.Count);
        for (int i = 0; i < randomRange; i++)
        {
            BaseLife.Name randomName = names[Random.Range(0, names.Count)];
            BaseLife enemy = LifeFactory.Create(randomName);

            enemy.AddDieEvent(OnEnemyDieRequested);
            _enemyCount++;
        }
    }
}