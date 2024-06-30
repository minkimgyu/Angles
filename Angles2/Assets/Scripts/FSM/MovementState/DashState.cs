using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DashState : State
{
    Action<Player.MovementState> SetState;
    Action<Vector2, float> AddForce;

    float _dashSpeed;
    float _dashDuration;

    Timer _timer;

    public DashState(float dashSpeed, float dashDuration, 
        Action<Player.MovementState> SetState, Action<Vector2, float> AddForce)
    {
        _dashSpeed = dashSpeed;
        _dashDuration = dashDuration;

        _timer = new Timer();

        this.SetState = SetState;
        this.AddForce = AddForce;
    }

    public override void OnStateEnter(Vector2 direction, string message)
    {
        _timer.Reset();
        Debug.Log(message);

        AddForce?.Invoke(direction, _dashSpeed);
        _timer.Start(_dashDuration);
    }

    public override void OnStateUpdate()
    {
        if (_timer.CurrentState != Timer.State.Finish) return;

        SetState?.Invoke(Player.MovementState.Move);
    }
}
