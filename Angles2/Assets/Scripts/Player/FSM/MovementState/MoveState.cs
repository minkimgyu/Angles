using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class MoveState : State<Player.MovementState>
    {
        Action<Vector2, float> Move;
        Func<bool> CanUseDash;
        Action UseDash;

        Vector2 _storedInput;
        float _moveSpeed;

        public MoveState(
            FSM<Player.MovementState> fsm,
            float moveSpeed,

            Func<bool> CanUseDash,
            Action UseDash,

            Action<Vector2, float> Move) : base(fsm)
        {
            _moveSpeed = moveSpeed;

            this.CanUseDash = CanUseDash;
            this.UseDash = UseDash;
            this.Move = Move;
        }

        public override void OnMove(Vector2 input)
        {
            _storedInput = input;
        }

        public override void OnFixedUpdate()
        {
            Move?.Invoke(_storedInput, _moveSpeed);
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
}