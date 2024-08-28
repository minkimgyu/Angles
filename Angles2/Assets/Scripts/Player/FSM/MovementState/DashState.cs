using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class DashState : State<Player.MovementState>
    {
        Action<bool, float> ChangeBodyScale;
        Action EndDash;
        Action<Vector2, float> AddForce;

        float _dashSpeed;
        float _dashDuration;
        MoveComponent _moveComponent;
        Timer _timer;

        public DashState(
            FSM<Player.MovementState> fsm,
            float dashSpeed, 
            float dashDuration,
            MoveComponent moveComponent,
            Action<bool, float> ChangeBodyScale,
            Action EndDash)
        : base(fsm)
        {
            _dashSpeed = dashSpeed;
            _dashDuration = dashDuration;

            _timer = new Timer();

            this.ChangeBodyScale = ChangeBodyScale;

            this.EndDash = EndDash;
            _moveComponent = moveComponent;
        }

        public override void OnStateEnter(Vector2 direction, string message)
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Dash);

            _timer.Reset();
            Debug.Log(message);
            ChangeBodyScale?.Invoke(false, 0);

            _moveComponent.AddForce(direction, _dashSpeed);
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

            _baseFSM.SetState(Player.MovementState.Move);
        }
    }

}