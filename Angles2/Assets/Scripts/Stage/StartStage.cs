using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : BaseStage
{
    [SerializeField] List<Transform> _bonusPostions;

    public override void Spawn(StageSpawnData data)
    {
        OnClearRequested?.Invoke();
    }
}