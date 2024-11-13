using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    public enum Type
    {
        StartStage,
        BonusStage,
        MobStage,
        BossStage,
    }

    [SerializeField] protected Transform _entryPoint;

    protected List<GameObject> _spawnedObjects;

    protected BaseStageController _baseStageController;
    protected CoreSystem _coreSystem;

    public virtual void Initialize(BaseStageController baseStageController, CoreSystem coreSystem) 
    {
        _spawnedObjects = new List<GameObject>();
        _baseStageController = baseStageController;
        _coreSystem = coreSystem;
    }

    public virtual void ActivePortal(Vector2 movePos) { }

    public virtual void AddBossHPEvent(Action<float> OnHPChange) { }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }

    public virtual void Spawn(int totalStageCount, int currentStageCount) { }

    public virtual void Exit() 
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            Destroy(_spawnedObjects[i]);
        }
    }
}