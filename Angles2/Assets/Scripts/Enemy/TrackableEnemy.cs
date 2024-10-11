using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TrackableEnemy : BaseEnemy
{
    public enum State
    {
        Wandering,
        Tracking
    }

    protected FSM<State> _fsm;
    protected List<ITarget.Type> _followableTypes;

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

    protected virtual void OnTargetEnter(ITarget target)
    {
        _fsm.OnTargetEnter(target);
    }

    protected virtual void OnTargetExit(ITarget target)
    {
        _fsm.OnTargetExit(target);
    }
}