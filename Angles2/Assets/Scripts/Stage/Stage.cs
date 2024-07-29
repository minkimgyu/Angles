using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StageSpawnData
{
    public StageSpawnData(int totalStageCount, int currentStageCount)
    {
        _totalStageCount = totalStageCount;
        _currentStageCount = currentStageCount;
    }

    int _totalStageCount;
    int _currentStageCount;

    public float ProgressRatio { get { return _currentStageCount / _totalStageCount; } }
}

public class BaseStage : MonoBehaviour
{
    public enum Type
    {
        Start,
        Battle,
        Bonus
    }

    protected System.Action OnClearRequested;

    [SerializeField] Transform _entryPoint;
    protected Portal _exitPortal;

    public void ActivePortal(Vector2 movePos)
    {
        _exitPortal.Active(movePos);
    }

    public virtual void Initialize(System.Action OnClearRequested)
    {
        this.OnClearRequested = OnClearRequested;

        //InteractableObjectFactory.Create();
        _exitPortal.Initialize();
    }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }

    public virtual void Spawn(StageSpawnData data) { }
}