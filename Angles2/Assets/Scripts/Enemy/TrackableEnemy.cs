using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableEnemy : BaseEnemy
{
    public enum State
    {
        Wandering,
        Tracking
    }

    FSM<State> _fsm;
    List<ITarget.Type> _followableTypes;

    ITarget _followTarget;
    [SerializeField] TargetCaptureComponent _followTargetCaptureComponent;
    protected float _stopDistance;
    protected float _gap;

    public override void Initialize()
    {
        base.Initialize();
        _followableTypes = new List<ITarget.Type> { ITarget.Type.Blue };
        _followTargetCaptureComponent.Initialize(OnTargetEnter, OnTargetExit);
        _fsm = new FSM<State>();
    }

    protected override void Update()
    {
        base.Update();

        if (_aliveState == AliveState.Groggy) return;
        _fsm.OnUpdate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_aliveState == AliveState.Groggy) return;
        _fsm.OnFixedUpdate();
    }

    public override void AddPathfindEvent(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
    {
        _fsm.Initialize(
           new Dictionary<State, BaseState<State>>
           {
               { State.Wandering, new WanderingState(_fsm, _moveComponent, transform, _followableTypes, 3, _moveSpeed, _moveSpeed) },
               { State.Tracking, new TrackingState(_fsm, _moveComponent, transform, _size, _moveSpeed, _stopDistance, _gap, FindPath) }
           },
           State.Wandering
        );
    }

    protected virtual void OnTargetEnter(ITarget target)
    {
        _fsm.OnTargetEnter(target);
    }

    protected virtual void OnTargetExit(ITarget target)
    {
        _fsm.OnTargetExit(target);
    }
}