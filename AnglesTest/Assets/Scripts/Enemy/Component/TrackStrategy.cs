using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackStrategy : IMoveStrategy
{
    //BaseLife.Size _size;
    protected float _moveSpeed;

    //Timer _pathfinderTimer;
    //float _pathfindGap = 0.5f;

    Transform _myTransform;
    MoveComponent _moveComponent;
    //Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath;

    TrackComponent _trackComponent;

    public TrackStrategy(Transform myTransform, MoveComponent moveComponent, TrackComponent trackComponent, float moveSpeed)
    {
        _myTransform = myTransform;
        _moveComponent = moveComponent;
        _trackComponent = trackComponent;
        //_size = size;
        _moveSpeed = moveSpeed;
        //_stopDistance = stopDistance;
        //_gap = gap;

        //_myTransform = myTransform;
        //_moveComponent = moveComponent;

        _state = State.Follow;
        //_movePoints = new List<Vector2>();
        //_pathfinderTimer = new Timer();
        //this.FindPath = FindPath;
    }

    public enum State
    {
        Stop,
        Follow,
    }

    State _state;
    ITarget _target;

    float _stopDistance;
    float _gap;

    public virtual void InjectTarget(ITarget target)
    {
        _target = target;
        _trackComponent.InjectTarget(target);
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath)
    {
        _trackComponent.InjectPathFindEvent(FindPath);
    }

    float ReturnDistanceBetweenTarget()
    {
        if (_target as UnityEngine.Object == null)
        {
            return 0;
        }

        Vector3 targetPos = _target.GetPosition();
        return Vector2.Distance(_myTransform.position, targetPos);
    }

    public virtual void OnUpdate()
    {
        if (_target as UnityEngine.Object == null) return;

        float diatance;
        switch (_state)
        {
            case State.Stop:
                diatance = ReturnDistanceBetweenTarget();
                if (diatance <= _stopDistance + _gap) break;

                _moveComponent.FreezeRotation(false);
                _state = State.Follow;

                break;
            case State.Follow:
                diatance = ReturnDistanceBetweenTarget();
                if (diatance < _stopDistance)
                {
                    _moveComponent.FreezeRotation(true);
                    _state = State.Stop;
                    break;
                }

                _trackComponent.OnUpdate();
                break;
            default:
                break;
        }
    }

    public void OnDrawGizmo()
    {
#if UNITY_EDITOR
        for (int i = 1; i < _trackComponent.MovePoints.Count; i++)
        {
            Debug.DrawLine(_trackComponent.MovePoints[i - 1], _trackComponent.MovePoints[i], Color.cyan);
        }
#endif
    }

    public virtual void OnFixedUpdate()
    {
        switch (_state)
        {
            case State.Stop:
                _moveComponent.Stop();
                break;
            case State.Follow:
                _moveComponent.Move(_trackComponent.MoveDirection, _moveSpeed);
                break;
            default:
                break;
        }
    }
}