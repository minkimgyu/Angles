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

        float _shootSpeed;
        float _shootDuration;

        float _ratio;
        float _maxChargePower;

        Timer _timer;
        Transform _myTransform;
        MoveComponent _moveComponent;

        public ShootState(
            FSM<Player.ActionState> fsm,
            float shootSpeed, 
            float shootDuration,
            float maxChargePower,

            Transform myTransform,
            MoveComponent moveComponent,

            Action<bool, float> ChangeBodyScale,
            Action<Collision2D> OnReflect,

            Action<bool> SetInvincible) : base(fsm)
        {
            _shootSpeed = shootSpeed;
            _shootDuration = shootDuration;
            _myTransform = myTransform;
            _moveComponent = moveComponent;
            _maxChargePower = maxChargePower;

            this.ChangeBodyScale = ChangeBodyScale;
            this.OnReflect = OnReflect;

            _timer = new Timer();
            this.SetInvincible = SetInvincible;
        }

        public override void OnCollisionEnter(Collision2D collision)
        {
            OnReflect?.Invoke(collision);

            Vector2 reflectDirection = Vector2.Reflect(_myTransform.right, collision.contacts[0].normal);
            _myTransform.right = reflectDirection;

            Debug.DrawRay(_myTransform.position, reflectDirection, Color.red, 5);

            Shoot(reflectDirection * _ratio * _maxChargePower);
        }

        public override void OnStateEnter(Vector2 direction, float ratio, string message)
        {
            _ratio = ratio;

            _timer.Reset();
            _timer.Start(_shootDuration);

            ChangeBodyScale?.Invoke(false, 0);
            Shoot(direction * _ratio * _maxChargePower);
        }

        public override void OnStateUpdate()
        {
            if(_timer.CurrentState == Timer.State.Finish)
            {
                GoToReadyState();
                return;
            }
        }

        // move가 검출된다면 바로 Ready로 보냄
        public override void OnMove(Vector2 vec2)
        {
            GoToReadyState();
        }

        void Shoot(Vector2 direction)
        {
            _moveComponent.Stop();
            _moveComponent.AddForce(direction, _shootSpeed);
        }

        void GoToReadyState()
        {
            SetInvincible?.Invoke(false);

            ChangeBodyScale?.Invoke(false, 1);
            _baseFSM.SetState(Player.ActionState.Ready);
        }
    }
}