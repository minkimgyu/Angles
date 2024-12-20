using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BattleStage : BaseStage
{
    protected int _bossCount = 0;
    protected Pathfinder _pathfinder;

    public override void Initialize(BaseStageController baseStageController, CoreSystem coreSystem)
    {
        base.Initialize(baseStageController, coreSystem);
        _pathfinder = GetComponent<Pathfinder>();

        GridComponent gridComponent = GetComponent<GridComponent>();
        gridComponent.Initialize(_pathfinder);
    }

    protected abstract void OnEnemyDieRequested();
}
