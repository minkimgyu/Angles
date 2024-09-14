using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveState : State<Player.MovementState>
{
    Func<bool> CanUseDash;
    Action UseDash;

    MoveComponent _moveComponent;
    Vector2 _storedInput;
    float _moveSpeed;

    public MoveState(
        FSM<Player.MovementState> fsm,
        float moveSpeed,

        Func<bool> CanUseDash,
        Action UseDash,

        MoveComponent moveComponent) : base(fsm)
    {
        _moveSpeed = moveSpeed;
        _moveComponent = moveComponent;

        this.CanUseDash = CanUseDash;
        this.UseDash = UseDash;
    }

    public override void OnMove(Vector2 input)
    {
        _storedInput = input;
    }

    public override void OnFixedUpdate()
    {
        _moveComponent.Move(_storedInput, _moveSpeed);
    }

    public override void OnMoveEnd()
    {
        _baseFSM.SetState(Player.MovementState.Stop);
    }

    public override void OnDash()
    {
        bool canUseDash = CanUseDash();
        if (canUseDash == false) return;

        UseDash?.Invoke();
        _baseFSM.SetState(Player.MovementState.Dash, _storedInput, "GoToDashState");
    }
}