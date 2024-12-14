using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FreezeTrackingState : TrackingState
{
    bool _nowFreeze;

    Timer _moveTimer;
    float _moveDuration;

    Timer _freezeTimer;
    float _freezeDuration;

    public FreezeTrackingState(
        FSM<TrackableEnemy.State> baseFSM,
        MoveComponent moveComponent,
        Transform myTransform,
        BaseLife.Size size,
        float moveSpeed,
        float stopDistance,
        float gap,

        float freezeDuration,
        float movableDuration,

        Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath) : base(baseFSM, moveComponent, myTransform, size, moveSpeed, stopDistance, gap, FindPath)
    {
        _nowFreeze = false;

        _freezeDuration = freezeDuration;
        _moveDuration = movableDuration;

        _freezeTimer = new Timer();
        _moveTimer = new Timer();
    }

    public override void OnStateEnter(ITarget target, string message)
    {
       base.OnStateEnter(target, message);
        _moveTimer.Start(_moveDuration);
    }

    public override void OnStateExit()
    {
        _moveTimer.Reset();
        _freezeTimer.Reset();
    }

    public override void OnFixedUpdate()
    {
        if (_nowFreeze == true)
        {
            _moveComponent.Stop();
            return;
        }

        base.OnFixedUpdate();
    }

    public override void OnStateUpdate()
    {
        if(_moveTimer.CurrentState == Timer.State.Finish)
        {
            _nowFreeze = true;
            
            _moveTimer.Reset();
            _freezeTimer.Start(_freezeDuration);
        }
        else if (_freezeTimer.CurrentState == Timer.State.Finish)
        {
            _nowFreeze = false;
            _freezeTimer.Reset();
            _moveTimer.Start(_moveDuration);
        }

        if (_nowFreeze == true) return;
        base.OnStateUpdate();
    }
}
