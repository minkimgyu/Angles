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

    const float _chargeMoveSpeedRatio = 0.5f;
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

    const float minAlpha = 0.03f;
    const float maxAlpha = 0.3f;
    const float _minChargeRatio = 0.25f;

    bool CanShoot(Vector2 input)
    {
        return input.magnitude >= _playerData.MinJoystickLength && _chargeTimer.Ratio >= _minChargeRatio;
    }

    public override void OnStateEnter()
    {
        _moveComponent.MoveSpeedRatio = _chargeMoveSpeedRatio;
        _chargeTimer.Start(_playerData.ChargeDuration);

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

        float handleAlpha = 0;
        if (_chargeTimer.Ratio >= _minChargeRatio) handleAlpha = maxAlpha;
        else handleAlpha = minAlpha;

        EventBusManager.Instance.ObserverEventBus.Publish(ObserverEventBus.State.OnChargeRatioChange, _chargeTimer.Ratio, handleAlpha);
    }

    public override void OnCharge(Vector2 input)
    {
        _storedInput = -input;
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
            ChangeBodyScale?.Invoke(true, 1); // 몸 크기 원래대로 바꾸기
            _baseFSM.SetState(Player.ActionState.Ready);
        }
    }
}
