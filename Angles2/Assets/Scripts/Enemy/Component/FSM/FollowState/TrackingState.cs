using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class TrackingState : State<TrackableEnemy.State>
{
    float _moveSpeed;

    Timer _pathfinderTimer;
    float _pathfindGap = 1f;

    Transform _myTransform;
    MoveComponent _moveComponent;
    Func<Vector2, Vector2, List<Vector2>> FindPath;

    public TrackingState(FSM<TrackableEnemy.State> baseFSM, MoveComponent moveComponent, Transform myTransform, float moveSpeed, Func<Vector2, Vector2, List<Vector2>> FindPath) : base(baseFSM)
    {
        _moveSpeed = moveSpeed;

        _myTransform = myTransform;
        _moveComponent = moveComponent;

        this.FindPath = FindPath;
        _movePoints = new List<Vector2>();
        _pathfinderTimer = new Timer();
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


    public override void OnStateEnter(ITarget target)
    {
        _target = target;
        _pathfinderTimer.Start(_pathfindGap);
    }

    public override void OnTargetExit()
    {
        _target = null;
        _pathfinderTimer.Reset();
        _baseFSM.SetState(TrackableEnemy.State.Wandering);
    }

    float ReturnDistanceBetweenTarget()
    {
        Vector3 targetPos = _target.ReturnPosition();
        return Vector2.Distance(_target.ReturnPosition(), targetPos);
    }

    public override void OnStateUpdate()
    {
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

                if(_pathfinderTimer.CurrentState == Timer.State.Finish)
                {
                    _movePoints = FindPath(_myTransform.position, _target.ReturnPosition());
                    _index = 0;

                    _pathfinderTimer.Reset();
                    _pathfinderTimer.Start(_pathfindGap);
                }

                ChangeDirection();
                break;
            default:
                break;
        }
    }

    List<Vector2> _movePoints;
    int _index = 0;
    Vector2 dir;



    void ChangeDirection()
    {
        if (_movePoints == null) return;

        Vector2.Distance()

        Vector2 nextMovePos = _movePoints[_index];
        Vector2 dir = (nextMovePos - (Vector2)_myTransform.position).normalized;

        _moveComponent.Move(dir, _moveSpeed);
    }

    public override void OnFixedUpdate()
    {
        switch (_state)
        {
            case State.Stop:
                _moveComponent.Stop();
                break;
            case State.Follow:
                _moveComponent.Move(dir, _moveSpeed);
                break;
            default:
                break;
        }
    }
}