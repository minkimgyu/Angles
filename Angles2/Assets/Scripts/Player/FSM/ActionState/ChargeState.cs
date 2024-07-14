using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player.FSM
{
    public class ChargeState : State<Player.ActionState>
    {
        Action<bool, float> ChangeBodyScale;
        Action<bool> SetInvincible;

        Action<bool> ShowShootDirection;
        Action<Vector3, Vector2> UpdateShootDirection;
        Action<Vector2> FaceDirection;

        float _minShootValue;
        Transform _myTransform;
        Vector2 _storedInput;

        public ChargeState(FSM<Player.ActionState> fsm, float minShootValue, Transform myTransform,

            Action<bool, float> ChangeBodyScale,

            Action<bool> SetInvincible,
            Action<bool> ShowShootDirection,
            Action<Vector3, Vector2> UpdateShootDirection,

            Action<Vector2> FaceDirection) : base(fsm)
        {
            _minShootValue = minShootValue;
            _myTransform = myTransform;

            this.ChangeBodyScale = ChangeBodyScale;

            this.SetInvincible = SetInvincible;
            this.ShowShootDirection = ShowShootDirection;
            this.UpdateShootDirection = UpdateShootDirection;

            this.FaceDirection = FaceDirection;
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
                _baseFSM.SetState(Player.ActionState.Shoot, _storedInput, "GoToShootState");
            }
            else
            {
                _baseFSM.SetState(Player.ActionState.Ready);
            }
        }
    }
}