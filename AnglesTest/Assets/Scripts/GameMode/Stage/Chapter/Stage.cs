using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    public enum Name
    {
        StartStage,
        BonusStage,
        BossStage,

        MobStage1,
        MobStage2,
        MobStage3,
        MobStage4,
        MobStage5,
    }

    [SerializeField] protected Transform _entryPoint;

    protected List<GameObject> _spawnedObjects;

    protected BaseStageController _baseStageController;
    protected InGameFactory _inGameFactory;

    public virtual void ResetData(MobStageData mobStageData) { }
    public virtual void ResetData(BossStageData bossStageData) { }

    public virtual void Initialize(BaseStageController baseStageController, AddressableHandler addressableHandler, InGameFactory inGameFactory) 
    {
        _spawnedObjects = new List<GameObject>();
        _baseStageController = baseStageController;
        _inGameFactory = inGameFactory;
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