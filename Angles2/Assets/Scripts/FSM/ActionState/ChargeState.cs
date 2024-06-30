using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargeState : State
{
    Action<bool> ApplyDirection;

    Action<Vector2> FaceDirection;

    Action<Player.ActionState> SetState;
    Action<Player.ActionState, Vector2, string> GoToShootState;

    float _minShootValue;
    Vector2 _storedInput;

    public ChargeState(float minShootValue,

        Action<bool> ApplyDirection,

        Action<Vector2> FaceDirection,

        Action<Player.ActionState> SetState,
        Action<Player.ActionState, Vector2, string> GoToShootState)
    {
        _minShootValue = minShootValue;

        this.ApplyDirection = ApplyDirection;

        this.FaceDirection = FaceDirection;

        this.SetState = SetState;
        this.GoToShootState = GoToShootState;
    }

    bool CanShoot(Vector2 input)
    {
        return input.magnitude >= _minShootValue;
    }

    public override void OnStateEnter()
    {
        ApplyDirection?.Invoke(true);
    }

    public override void OnStateExit()
    {

    }

    public override void OnCharge(Vector2 input)
    {
        _storedInput = -input;
        FaceDirection?.Invoke(-input);
    }

    public override void OnChargeEnd()
    {
        Debug.Log("OnChargeEnd");

        ApplyDirection?.Invoke(false);

        bool canShoot = CanShoot(_storedInput);

        if(canShoot) GoToShootState?.Invoke(Player.ActionState.Shoot, _storedInput, "GoToShootState");
        else SetState?.Invoke(Player.ActionState.Ready);
    }
}
