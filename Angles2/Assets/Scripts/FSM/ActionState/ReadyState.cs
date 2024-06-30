using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReadyState : State
{
    Action<Player.ActionState> SetState;

    public ReadyState(Action<Player.ActionState> SetState)
    {
        this.SetState = SetState;
    }

    public override void OnCharge(Vector2 input)
    {
        SetState?.Invoke(Player.ActionState.Charge);
    }
}
