using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ShootState : State
    {
        Action<bool, float> ChangeBodyScale;

        Action<bool> SetInvincible;
        Action<Player.ActionState> SetState;
        Action<Player.ActionState, Vector2, string> GoToReflectState;

        Action<Collision2D> OnReflect;

        Action<Vector2, float> AddForce;

        float _shootSpeed;
        float _shootDuration;

        Timer _timer;

        public ShootState(float shootSpeed, float shootDuration,

            Action<bool, float> ChangeBodyScale,
            Action<Collision2D> OnReflect,

            Action<bool> SetInvincible,
            Action<Player.ActionState> SetState,
            Action<Player.ActionState, Vector2, string> GoToReflectState,
            Action<Vector2, float> AddForce)
        {
            _shootSpeed = shootSpeed;
            _shootDuration = shootDuration;

            this.ChangeBodyScale = ChangeBodyScale;
            this.OnReflect = OnReflect;

            _timer = new Timer();

            this.SetInvincible = SetInvincible;

            this.SetState = SetState;
            this.GoToReflectState = GoToReflectState;
            this.AddForce = AddForce;
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            OnReflect?.Invoke(collision);
            GoToReflectState?.Invoke(Player.ActionState.Reflect, collision.contacts[0].normal, "GoToReflect");
        }

        public override void OnStateEnter(Vector2 direction, string message)
        {
            _timer.Reset();
            ChangeBodyScale?.Invoke(false, 0);

            AddForce?.Invoke(direction, _shootSpeed);
            _timer.Start(_shootDuration);
        }

        // move가 검출된다면 바로 Ready로 보냄
        public override void OnMove(Vector2 vec2)
        {
            SetInvincible?.Invoke(false);

            ChangeBodyScale?.Invoke(false, 1);
            SetState?.Invoke(Player.ActionState.Ready);
        }
    }
}