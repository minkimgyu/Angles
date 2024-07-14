using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ShootState : State<Player.ActionState>
    {
        Action<bool, float> ChangeBodyScale;

        Action<bool> SetInvincible;
        Action<Collision2D> OnReflect;

        Action<Vector2, float> AddForce;
        Action Stop;

        float _shootSpeed;
        float _shootDuration;

        Timer _timer;

        public ShootState(
            FSM<Player.ActionState> fsm,
            float shootSpeed, float shootDuration,
            Action<bool, float> ChangeBodyScale,
            Action<Collision2D> OnReflect,

            Action<bool> SetInvincible,

            Action Stop,
            Action<Vector2, float> AddForce) : base(fsm)
        {
            _shootSpeed = shootSpeed;
            _shootDuration = shootDuration;

            this.ChangeBodyScale = ChangeBodyScale;
            this.OnReflect = OnReflect;

            _timer = new Timer();

            this.SetInvincible = SetInvincible;

            this.Stop = Stop;
            this.AddForce = AddForce;
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            OnReflect?.Invoke(collision);
            _baseFSM.SetState(Player.ActionState.Reflect, collision.contacts[0].normal, "GoToReflect");
        }

        public override void OnStateEnter(Vector2 direction, string message)
        {
            _timer.Reset();
            ChangeBodyScale?.Invoke(false, 0);

            Stop?.Invoke();
            AddForce?.Invoke(direction, _shootSpeed);
            _timer.Start(_shootDuration);
        }

        // move가 검출된다면 바로 Ready로 보냄
        public override void OnMove(Vector2 vec2)
        {
            SetInvincible?.Invoke(false);

            ChangeBodyScale?.Invoke(false, 1);
            _baseFSM.SetState(Player.ActionState.Ready);
        }
    }
}