using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleTrackingStrategy : TrackStrategy
{
    bool _nowFreeze;

    Timer _moveTimer;
    float _moveDuration;

    float _freezeSpeed;
    float _normalSpeed;

    Timer _freezeTimer;
    float _freezeDuration;

    public FlexibleTrackingStrategy(
        Transform myTransform,
        MoveComponent moveComponent,
        TrackComponent trackComponent,

        float freezeSpeed,
        float normalSpeed,

        float freezeDuration,
        float movableDuration) : base(myTransform, moveComponent, trackComponent, normalSpeed)
    {
        _nowFreeze = false;

        _freezeSpeed = freezeSpeed;
        _normalSpeed = normalSpeed;

        _freezeDuration = freezeDuration;
        _moveDuration = movableDuration;

        _freezeTimer = new Timer();
        _moveTimer = new Timer();
    }

    public override void InjectTarget(ITarget target)
    {
        base.InjectTarget(target);
        _moveTimer.Start(_moveDuration);
    }

    public override void OnFixedUpdate()
    {
        if (_nowFreeze == true)
        {
            _moveSpeed = _freezeSpeed;
        }
        else
        {
            _moveSpeed = _normalSpeed;
        }

        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        if (_moveTimer.CurrentState == Timer.State.Finish)
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
