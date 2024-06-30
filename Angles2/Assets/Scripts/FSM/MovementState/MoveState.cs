using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveState : State
{
    Action<Vector2, float> Move;
    Action<Player.MovementState> SetState;
    Action<Player.MovementState, Vector2, string> GoToDashState;

    Func<bool> CanUseDash;
    Action UseDash;

    Vector2 _storedInput;
    float _moveSpeed;

    public MoveState(
        float moveSpeed,

        Func<bool> CanUseDash,
        Action UseDash,

        Action<Vector2, float> Move,
        Action<Player.MovementState> SetState,
        Action<Player.MovementState, Vector2, string> GoToDashState)
    {
        _moveSpeed = moveSpeed;

        this.CanUseDash = CanUseDash;
        this.UseDash = UseDash;

        this.Move = Move;
        this.SetState = SetState;
        this.GoToDashState = GoToDashState;
    }

    public override void OnMove(Vector2 input)
    {
        _storedInput = input;
        Move?.Invoke(input, _moveSpeed);
    }

    public override void OnMoveEnd()
    {
        SetState?.Invoke(Player.MovementState.Stop);
    }

    public override void OnDash()
    {
        bool canUseDash = CanUseDash();
        if(canUseDash == false) return;

        UseDash?.Invoke();
        GoToDashState?.Invoke(Player.MovementState.Dash, _storedInput, "GoToDashState");
    }
}
