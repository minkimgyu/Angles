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
    List<ITarget.Type> _followableType;

    ITarget _followTarget;
    TargetCaptureComponent _targetCaptureComponent;

    public override void Initialize()
    {
        base.Initialize();
        _targetCaptureComponent = GetComponentInChildren<TargetCaptureComponent>();
        _targetCaptureComponent.Initialize(OnEnter, OnExit);

        _fsm = new FSM<State>();
        _fsm.Initialize
        (
           new Dictionary<State, BaseState<State>>
           {
               { State.Wandering, new WanderingState(_fsm) },
               { State.Tracking, new TrackingState(_fsm) }
           }
        );
    }

    private void OnExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_followableType);
        if (isTarget == false) return;

        _fsm.OnTargetEnter(target);
    }

    private void OnEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_followableType);
        if (isTarget == false) return;

        _fsm.OnTargetExit();
    }
}
