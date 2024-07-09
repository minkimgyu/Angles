using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class StopState : State
    {
        Action<Player.MovementState> SetState;
        Action Stop;

        public StopState(Action<Player.MovementState> SetState, Action Stop)
        {
            this.SetState = SetState;
            this.Stop = Stop;
        }

        public override void OnStateEnter()
        {
            Stop?.Invoke();
        }

        public override void OnMoveStart()
        {
            SetState?.Invoke(Player.MovementState.Move);
        }
    }
}