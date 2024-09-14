using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    public enum Type
    {
        Start,
        Bonus,
        Battle
    }

    [SerializeField] protected Transform _entryPoint;
    protected Portal _portal;

    protected List<GameObject> _spawnedObjects;

    protected BaseStageController _baseStageController;
    protected FactoryCollection _factoryCollection;

    public virtual void Initialize(BaseStageController baseStageController, FactoryCollection factoryCollection) 
    {
        _spawnedObjects = new List<GameObject>();
        _baseStageController = baseStageController;
        _factoryCollection = factoryCollection;

        Portal portal = GetComponentInChildren<Portal>();
        _portal.Initialize(_baseStageController.OnMoveToNextStageRequested);
    }

    public void ActivePortal(Vector2 movePos)
    {
        _portal.Active(movePos);
    }

    public IPos Target;

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

        _portal.Disable();
    }
}