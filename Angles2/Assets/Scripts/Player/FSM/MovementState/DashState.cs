using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class DashState : State
    {
        Action<bool, float> ChangeBodyScale;
        Action EndDash;

        Action<Player.MovementState> SetState;
        Action<Vector2, float> AddForce;

        float _dashSpeed;
        float _dashDuration;

        Timer _timer;

        public DashState(float dashSpeed, float dashDuration,

            Action<bool, float> ChangeBodyScale,

            Action EndDash, Action<Player.MovementState> SetState, Action<Vector2, float> AddForce)
        {
            _dashSpeed = dashSpeed;
            _dashDuration = dashDuration;

            _timer = new Timer();

            this.ChangeBodyScale = ChangeBodyScale;

            this.EndDash = EndDash;

            this.SetState = SetState;
            this.AddForce = AddForce;
        }

        public override void OnStateEnter(Vector2 direction, string message)
        {
            _timer.Reset();
            Debug.Log(message);
            ChangeBodyScale?.Invoke(false, 0);

            AddForce?.Invoke(direction, _dashSpeed);
            _timer.Start(_dashDuration);
        }

        public override void OnStateExit()
        {
            EndDash?.Invoke();
        }

        public override void OnStateUpdate()
        {
            ChangeBodyScale?.Invoke(false, _timer.Ratio);
            if (_timer.CurrentState != Timer.State.Finish) return;
            
            SetState?.Invoke(Player.MovementState.Move);
        }
    }

}