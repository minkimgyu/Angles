using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BattleStage : BaseStage
{
    protected int _enemyCount = 0;
    protected Pathfinder _pathfinder;

    public override void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection)
    {
        base.Initialize(baseStageController, factoryCollection);
        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize();
        _pathfinder = GetComponent<Pathfinder>();
    }

    protected abstract void OnEnemyDieRequested();
}
