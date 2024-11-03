using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargeState : State<Player.ActionState>
{
    Action<bool, float> ChangeBodyScale;
    Action<bool> SetInvincible;

    Transform _myTransform;
    Vector2 _storedInput;
    MoveComponent _moveComponent;

    Timer _chargeTimer;
    PlayerData _playerData;

    const float _chargeMoveSpeedRatio = 0.7f;
    const float _normalMoveSpeedRatio = 1.0f;

    public ChargeState(
        FSM<Player.ActionState> fsm,

        PlayerData playerData,
        //float minShootValue, 
        //BuffFloat chargeDuration,


        Transform myTransform,
        MoveComponent moveComponent,

        Action<bool, float> ChangeBodyScale,
        Action<bool> SetInvincible) : base(fsm)
    {

        _playerData = playerData;

        _myTransform = myTransform;
        _moveComponent = moveComponent;

        _chargeTimer = new Timer();

        this.ChangeBodyScale = ChangeBodyScale;
        this.SetInvincible = SetInvincible;

        //this.OnChargeRatioChangeRequested = OnChargeRatioChangeRequested;

        //this.ShowShootDirection = ShowShootDirection;
    }

    bool CanShoot(Vector2 input)
    {
        return input.magnitude >= _playerData._minJoystickLength;
    }

    public override void OnStateEnter()
    {
        _moveComponent.MoveSpeedRatio = _chargeMoveSpeedRatio;
        _chargeTimer.Start(_playerData._chargeDuration);

        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnTurnOnOffDirection, true);
        //ShowShootDirection?.Invoke(true);
        _moveComponent.ApplyDirection = false;
    }

    public override void OnStateExit()
    {
        _moveComponent.MoveSpeedRatio = _normalMoveSpeedRatio;
        _chargeTimer.Reset();
    }

    public override void OnStateUpdate()
    {
        ChangeBodyScale?.Invoke(true, 1 - _storedInput.magnitude);
        _moveComponent.FaceDirection(_storedInput);

        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnChargeRatioChange, _chargeTimer.Ratio);
    }

    public override void OnCharge(Vector2 input)
    {
        _storedInput = -input.normalized;
    }

    public override void OnChargeEnd()
    {
        //Debug.Log("OnChargeEnd");

        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnTurnOnOffDirection, false);
        //ShowShootDirection?.Invoke(false);
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
