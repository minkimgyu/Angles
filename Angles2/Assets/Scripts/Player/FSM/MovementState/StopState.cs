using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class StopState : State<Player.MovementState>
    {
        MoveComponent _moveComponent;

        public StopState(FSM<Player.MovementState> baseFSM, MoveComponent moveComponent) : base(baseFSM)
        {
            _moveComponent = moveComponent;
        }

        public override void OnStateEnter()
        {
            _moveComponent.Stop();
        }

        public override void OnMoveStart()
        {
            _baseFSM.SetState(Player.MovementState.Move);
        }
    }
}