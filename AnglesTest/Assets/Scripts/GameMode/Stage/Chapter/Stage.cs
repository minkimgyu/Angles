using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    [SerializeField] protected Transform _entryPoint;

    protected List<GameObject> _spawnedObjects;
    protected InGameFactory _inGameFactory;

    public virtual void ResetData(MobStageData mobStageData) { }
    public virtual void ResetData(BossStageData bossStageData) { }
    public virtual void ResetData(SurvivalStageData survivalStageData) { }

    public virtual void Initialize(
        GameMode gameMode,
        AddressableHandler addressableHandler,
        InGameFactory inGameFactory,
        SurvivalLevelUIController levelUIController,
        ArrowPointerController arrowPointerController)
    {
        _spawnedObjects = new List<GameObject>();
        _inGameFactory = inGameFactory;
    }

    public virtual void Initialize(
        BaseStageController baseStageController,
        AddressableHandler addressableHandler,
        InGameFactory inGameFactory) 
    {
        _spawnedObjects = new List<GameObject>();
        _inGameFactory = inGameFactory;
    }

    public virtual void AddPlayer(ITarget target) { }

    public virtual void ActivePortal(Vector2 movePos = default) { }
    public virtual void AddBossHPEvent(Action<float> OnHPChange) { }

    public virtual void Spawn(float passedTime) { }
    public virtual void Spawn(int totalStageCount, int currentStageCount) { }

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }

    public virtual void Exit() 
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            Destroy(_spawnedObjects[i]);
        }
    }
}