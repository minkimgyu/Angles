using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackComponent : IMoveStrategy
{
    BaseLife.Size _size;
    protected float _moveSpeed;

    Timer _pathfinderTimer;
    float _pathfindGap = 0.5f;

    Transform _myTransform;
    MoveComponent _moveComponent;
    Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath;

    public TrackComponent(
        MoveComponent moveComponent,
        Transform myTransform,
        BaseLife.Size size,
        float moveSpeed,
        float stopDistance,
        float gap,
        Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath)
    {
        _size = size;
        _moveSpeed = moveSpeed;
        _stopDistance = stopDistance;
        _gap = gap;

        _myTransform = myTransform;
        _moveComponent = moveComponent;

        _state = State.Follow;
        _movePoints = new List<Vector2>();
        _pathfinderTimer = new Timer();
        this.FindPath = FindPath;
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

    public virtual void AddTarget(ITarget target)
    {
        _target = target;
        _pathfinderTimer.Start(_pathfindGap);
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

                if (_target as UnityEngine.Object == null) return;

                if (_pathfinderTimer.CurrentState == Timer.State.Finish)
                {
                    _movePoints = FindPath(_myTransform.position, _target.GetPosition(), _size);
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
    float _closeDistance = 0.5f;
    Vector2 _dir;

    void ChangeDirection()
    {
        if (_movePoints == null) return;
        if (_index >= _movePoints.Count)
        {
            _dir = Vector2.zero;
            return;
        }

        DrawMovePoints();

        Vector2 nextMovePos = _movePoints[_index];
        _dir = (nextMovePos - (Vector2)_myTransform.position).normalized;

        bool nowCloseToNextPoint = Vector2.Distance(_myTransform.position, nextMovePos) < _closeDistance;
        if (nowCloseToNextPoint) _index++;
    }

    void DrawMovePoints()
    {
#if UNITY_EDITOR
        for (int i = 1; i < _movePoints.Count; i++)
        {
            Debug.DrawLine(_movePoints[i - 1], _movePoints[i], Color.cyan);
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
                _moveComponent.Move(_dir, _moveSpeed);
                break;
            default:
                break;
        }
    }
}