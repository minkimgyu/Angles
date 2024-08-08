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

        Action<float> OnChargeRatioChangeRequested;

        Action<bool> ShowShootDirection;
        Action<Vector3, Vector2> UpdateShootDirection;

        float _minShootValue;
        Transform _myTransform;
        Vector2 _storedInput;
        MoveComponent _moveComponent;

        Timer _chargeTimer;
        float _chargeDuration;
        float _maxChargePower;

        public ChargeState(
            FSM<Player.ActionState> fsm, 
            float minShootValue, 
            float chargeDuration,
            Transform myTransform,
            MoveComponent moveComponent,

            Action<bool, float> ChangeBodyScale,
            Action<bool> SetInvincible,

            Action<float> OnChargeRatioChangeRequested,
            Action<bool> ShowShootDirection,
            Action<Vector3, Vector2> UpdateShootDirection) : base(fsm)
        {
            _minShootValue = minShootValue;
            _myTransform = myTransform;
            _moveComponent = moveComponent;

            _chargeTimer = new Timer();
            _chargeDuration = chargeDuration;

            this.ChangeBodyScale = ChangeBodyScale;
            this.SetInvincible = SetInvincible;

            this.OnChargeRatioChangeRequested = OnChargeRatioChangeRequested;

            this.ShowShootDirection = ShowShootDirection;
            this.UpdateShootDirection = UpdateShootDirection;
        }

        bool CanShoot(Vector2 input)
        {
            return input.magnitude >= _minShootValue;
        }

        public override void OnStateEnter()
        {
            _chargeTimer.Start(_chargeDuration);

            ShowShootDirection?.Invoke(true);
            _moveComponent.ApplyDirection = false;
        }

        public override void OnStateExit()
        {
            _chargeTimer.Reset();
            OnChargeRatioChangeRequested?.Invoke(_chargeTimer.Ratio);
        }

        public override void OnStateUpdate()
        {
            ChangeBodyScale?.Invoke(true, 1 - _storedInput.magnitude);
            _moveComponent.FaceDirection(_storedInput);

            OnChargeRatioChangeRequested?.Invoke(_chargeTimer.Ratio);
            UpdateShootDirection?.Invoke(_myTransform.position, _storedInput);
        }

        public override void OnCharge(Vector2 input)
        {
            _storedInput = -input.normalized;
        }

        public override void OnChargeEnd()
        {
            Debug.Log("OnChargeEnd");

            ShowShootDirection?.Invoke(false);
            _moveComponent.ApplyDirection = true;

            bool canShoot = CanShoot(_storedInput);

            if (canShoot)
            {
                SetInvincible?.Invoke(true);
                _baseFSM.SetState(Player.ActionState.Shoot, _storedInput, _chargeTimer.Ratio, "GoToShootState");
            }
            else
            {
                _baseFSM.SetState(Player.ActionState.Ready);
            }
        }
    }
}