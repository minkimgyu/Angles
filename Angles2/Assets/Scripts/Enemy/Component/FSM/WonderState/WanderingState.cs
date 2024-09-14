using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingState : State<TrackableEnemy.State>
{
    public enum State
    {
        Stop,
        Move,
        Rotate
    }

    State _state;
    Timer _timer;

    List<ITarget.Type> _targetTypes;
    float _stateChangeTime;
    float _moveSpeed;
    float _rotationSpeed;

    MoveComponent _moveComponent;
    Transform _myTransform;

    Vector3 _randomDirection;

    public WanderingState(FSM<TrackableEnemy.State> baseFSM, MoveComponent moveComponent, Transform myTransform,
        List<ITarget.Type> targetTypes, float stateChangeTime, float moveSpeed, float rotationSpeed) : base(baseFSM)
    {
        _targetTypes = targetTypes;
        _stateChangeTime = stateChangeTime;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;

        _moveComponent = moveComponent;
        _myTransform = myTransform;

        _state = State.Stop;
        _timer = new Timer();
        _timer.Start(stateChangeTime);
    }

    public override void OnTargetEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        Debug.Log("Tracking");
        _baseFSM.SetState(TrackableEnemy.State.Tracking, target, "Find Target");
    }

    void ChangeToRandomState()
    {
        int stateCount = Enum.GetValues(typeof(State)).Length;
        int index = Random.Range(0, stateCount);
        _state = (State)index;

        switch (_state)
        {
            case State.Rotate:
                _randomDirection = _myTransform.forward;
                _randomDirection.x += Random.Range(-1.0f, 1.0f);
                _randomDirection.y += Random.Range(-1.0f, 1.0f);

                _randomDirection = _randomDirection.normalized;
                break;
        }
    }

    public override void OnStateUpdate()
    {
        if(_timer.CurrentState == Timer.State.Finish)
        {
            _timer.Reset();
            _timer.Start(_stateChangeTime);
            ChangeToRandomState();
        }

        switch (_state)
        {
            case State.Stop:
                _moveComponent.Stop();
                break;
            case State.Move:
                _moveComponent.FaceDirection(_randomDirection);
                _moveComponent.Move(_randomDirection, _moveSpeed);
                break;
            case State.Rotate:
                _moveComponent.FaceDirection(_randomDirection, _rotationSpeed);
                break;
        }
    }
}
