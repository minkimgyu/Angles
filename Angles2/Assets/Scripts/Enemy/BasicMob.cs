using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMob : TrackableEnemy
{
    public override void InitializeFSM(Func<Vector2, Vector2, Size, List<Vector2>> FindPath)
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
}
