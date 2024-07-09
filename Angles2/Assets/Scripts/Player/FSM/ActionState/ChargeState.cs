using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ChargeState : State
    {

        Action<bool, float> ChangeBodyScale;

        Action<bool> SetInvincible;

        Action<bool> ShowShootDirection;
        Action<Vector3, Vector2> UpdateShootDirection;

        Action<Vector2> FaceDirection;

        Action<Player.ActionState> SetState;
        Action<Player.ActionState, Vector2, string> GoToShootState;

        float _minShootValue;
        Transform _myTransform;
        Vector2 _storedInput;

        public ChargeState(float minShootValue, Transform myTransform,

            Action<bool, float> ChangeBodyScale,

            Action<bool> SetInvincible,
            Action<bool> ShowShootDirection,
            Action<Vector3, Vector2> UpdateShootDirection,

            Action<Vector2> FaceDirection,

            Action<Player.ActionState> SetState,
            Action<Player.ActionState, Vector2, string> GoToShootState)
        {
            _minShootValue = minShootValue;
            _myTransform = myTransform;

            this.ChangeBodyScale = ChangeBodyScale;

            this.SetInvincible = SetInvincible;
            this.ShowShootDirection = ShowShootDirection;
            this.UpdateShootDirection = UpdateShootDirection;

            this.FaceDirection = FaceDirection;

            this.SetState = SetState;
            this.GoToShootState = GoToShootState;
        }

        bool CanShoot(Vector2 input)
        {
            return input.magnitude >= _minShootValue;
        }

        public override void OnStateEnter()
        {
            ShowShootDirection?.Invoke(true);
        }

        public override void OnStateUpdate()
        {
            UpdateShootDirection?.Invoke(_myTransform.position, _storedInput);
        }

        public override void OnCharge(Vector2 input)
        {
            _storedInput = -input;

            ChangeBodyScale?.Invoke(true, 1 - input.magnitude);
            FaceDirection?.Invoke(-input);
        }

        public override void OnChargeEnd()
        {
            Debug.Log("OnChargeEnd");

            ShowShootDirection?.Invoke(false);

            bool canShoot = CanShoot(_storedInput);

            if (canShoot)
            {
                SetInvincible?.Invoke(true);
                GoToShootState?.Invoke(Player.ActionState.Shoot, _storedInput, "GoToShootState");
            }
            else SetState?.Invoke(Player.ActionState.Ready);
        }
    }
}