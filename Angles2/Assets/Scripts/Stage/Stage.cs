using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : MonoBehaviour
{
    public struct Events
    {
        public Events(
            DungeonSystem.CommandCollection commandCollection,
            DungeonSystem.ObserverEventCollection eventCollection,

            Action OnStageClearRequested, 
            Action OnMoveToNextStageRequested
        )
        {
            _commandCollection = commandCollection;
            _eventCollection = eventCollection;

            _OnStageClearRequested = OnStageClearRequested;
            _OnMoveToNextStageRequested = OnMoveToNextStageRequested;
        }

        DungeonSystem.CommandCollection _commandCollection;
        public DungeonSystem.CommandCollection CommandCollection { get { return _commandCollection; } }

        DungeonSystem.ObserverEventCollection _eventCollection;
        public DungeonSystem.ObserverEventCollection ObserberEventCollection { get { return _eventCollection; } }


        Action _OnStageClearRequested;
        public Action OnStageClearRequested { get { return _OnStageClearRequested; } }

        Action _OnMoveToNextStageRequested;
        public Action OnMoveToNextStageRequested { get { return _OnMoveToNextStageRequested; } }
    }

    public enum Type
    {
        Start,
        Bonus,
        Battle
    }

    [SerializeField] protected Transform _entryPoint;
    [SerializeField] protected Portal _portal;

    protected List<GameObject> _spawnedObjects;

    protected Events _events;

    public virtual void Initialize(Events events) 
    { 
        _events = events;
        _spawnedObjects = new List<GameObject>();
        _portal.Initialize(_events.OnMoveToNextStageRequested);
    }

    public void ActivePortal(Vector2 movePos)
    {
        _portal.Active(movePos);
        //Debug.Log("ActivePortal");
    }

    public IPos Target;

    public Vector3 ReturnEntryPosition()
    {
        return _entryPoint.position;
    }

    public virtual void Spawn(int totalStageCount, int currentStageCount, IFactory factory) { }

    public virtual void Exit() 
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            Destroy(_spawnedObjects[i]);
        }

        _portal.Disable();
    }
}