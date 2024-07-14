using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class StopState : State<Player.MovementState>
    {
        Action Stop;

        public StopState(FSM<Player.MovementState> baseFSM, Action Stop) : base(baseFSM)
        {
            this.Stop = Stop;
        }

        public override void OnStateEnter()
        {
            Stop?.Invoke();
        }

        public override void OnMoveStart()
        {
            _baseFSM.SetState(Player.MovementState.Move);
        }
    }
}