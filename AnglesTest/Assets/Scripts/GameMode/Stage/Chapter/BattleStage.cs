using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BattleStage : BaseStage
{
    protected int _enemyCount = 0;
    protected Pathfinder _pathfinder;

    public override void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(baseStageController, addressableHandler, inGameFactory);
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);
    }

    protected abstract void OnEnemyDieRequested();
}