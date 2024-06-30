using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShootState : State
{
    Action<Player.ActionState> SetState;
    Action<Player.ActionState, Vector2, string> GoToReflectState;

    Action<Vector2, float> AddForce;

    float _shootSpeed;
    float _shootDuration;

    Timer _timer;

    public ShootState(float shootSpeed, float shootDuration,
        Action<Player.ActionState> SetState,
        Action<Player.ActionState, Vector2, string> GoToReflectState,
        Action<Vector2, float> AddForce)
    {
        _shootSpeed = shootSpeed;
        _shootDuration = shootDuration;

        _timer = new Timer();

        this.SetState = SetState;
        this.GoToReflectState = GoToReflectState;
        this.AddForce = AddForce;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        GoToReflectState?.Invoke(Player.ActionState.Reflect, collision.contacts[0].point, "GoToReflect");
    }

    public override void OnStateEnter(Vector2 direction, string message)
    {
        _timer.Reset();

        AddForce?.Invoke(direction, _shootSpeed);
        _timer.Start(_shootDuration);
    }

    // move가 검출된다면 바로 Ready로 보냄
    public override void OnMove(Vector2 vec2)
    {
        SetState?.Invoke(Player.ActionState.Ready);
    }

    public override void OnStateUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input == Vector2.zero && _timer.CurrentState != Timer.State.Finish) return;

        SetState?.Invoke(Player.ActionState.Ready);
    }
}
