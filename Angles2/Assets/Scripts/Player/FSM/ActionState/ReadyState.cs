using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ReadyState : State<Player.ActionState>
    {
        public ReadyState(FSM<Player.ActionState> fsm) : base(fsm)
        {
        }

        public override void OnCharge(Vector2 input)
        {
            _baseFSM.SetState(Player.ActionState.Charge);
        }
    }
}