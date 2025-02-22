using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BattleStage : BaseStage
{
    protected int _enemyCount = 0;
    protected Pathfinder _pathfinder;
    protected BaseStageController _baseStageController;

    public override void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory)
    {
        base.Initialize(baseStageController, addressableHandler, inGameFactory);

        _baseStageController = baseStageController;
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);
    }

    protected abstract void OnEnemyDieRequested();
}
