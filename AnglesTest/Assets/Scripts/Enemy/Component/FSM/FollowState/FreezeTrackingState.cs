using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrackingComponent : TrackComponent
{
    bool _nowFreeze;

    Timer _moveTimer;
    float _moveDuration;

    Timer _freezeTimer;
    float _freezeDuration;

    public FreezeTrackingComponent(
        MoveComponent moveComponent,
        Transform myTransform,
        BaseLife.Size size,
        float moveSpeed,
        float stopDistance,
        float gap,

        float freezeDuration,
        float movableDuration,

        Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath) : base(moveComponent, myTransform, size, moveSpeed, stopDistance, gap, FindPath)
    {
        _nowFreeze = false;

        _freezeDuration = freezeDuration;
        _moveDuration = movableDuration;

        _freezeTimer = new Timer();
        _moveTimer = new Timer();
    }

    public override void AddTarget(ITarget target)
    {
       base.AddTarget(target);
        _moveTimer.Start(_moveDuration);
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

    public override void OnUpdate()
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
        base.OnUpdate();
    }
}
