using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackComponent : MonoBehaviour
{
    BaseLife.Size _size;

    Timer _pathfinderTimer;
    const float _pathfindGap = 0.5f;

    Transform _myTransform;
    Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath;

    int _index;
    const float _closeDistance = 0.5f;

    List<Vector2> _movePoints;
    public List<Vector2> MovePoints { get => _movePoints; }

    Vector2 _moveDirection;
    public Vector2 MoveDirection { get => _moveDirection; }

    ITarget _target;

    public void Initialize(
        Transform myTransform,
        BaseLife.Size size)
    {
        _myTransform = myTransform;
        _size = size;

        _index = 0;
        _movePoints = new List<Vector2>();
        _pathfinderTimer = new Timer();
        _pathfinderTimer.Start(_pathfindGap);

        _moveDirection = Vector2.zero;
    }

    public void InjectTarget(ITarget target)
    {
        _target = target;
    }

    public void InjectPathFindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath)
    {
        this.FindPath = FindPath;
    }

    public void OnUpdate()
    {
        if (_target as UnityEngine.Object == null) return;
        if (FindPath == null) return;

        if (_pathfinderTimer.CurrentState == Timer.State.Finish)
        {
            _movePoints = FindPath(_myTransform.position, _target.GetPosition(), _size);
            _index = 0;

            _pathfinderTimer.Reset();
            _pathfinderTimer.Start(_pathfindGap);
        }

        if (_movePoints == null || _movePoints.Count == 0) return;
        if (_index >= _movePoints.Count)
        {
            _moveDirection = Vector2.zero;
            return;
        }

        Vector2 nextMovePos = _movePoints[_index];
        _moveDirection = (nextMovePos - (Vector2)_myTransform.position).normalized;

        bool nowCloseToNextPoint = Vector2.Distance(_myTransform.position, nextMovePos) < _closeDistance;
        if (nowCloseToNextPoint) _index++;
    }
}