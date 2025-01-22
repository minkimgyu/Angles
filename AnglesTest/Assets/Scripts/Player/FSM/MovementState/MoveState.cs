using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveState : State<Player.MovementState>
{
    Func<bool> CanUseDash;
    Action UseDash;

    MoveComponent _moveComponent;
    Vector2 _storedInput;

    PlayerData _playerData;

    public MoveState(
        FSM<Player.MovementState> fsm,
        PlayerData playerData,

        Func<bool> CanUseDash,
        Action UseDash,

        MoveComponent moveComponent) : base(fsm)
    {
        _playerData = playerData;
        _moveComponent = moveComponent;

        this.CanUseDash = CanUseDash;
        this.UseDash = UseDash;
    }

    Action MoveStartTutorialEvent;

    public override void InjectTutorialEvent(Action MoveStartTutorialEvent)
    {
        this.MoveStartTutorialEvent = MoveStartTutorialEvent;
    }

    public override void OnStateEnter()
    {
        MoveStartTutorialEvent?.Invoke();
    }

    public override void OnMove(Vector2 input)
    {
        _storedInput = input;
    }

    public override void OnFixedUpdate()
    {
        _moveComponent.Move(_storedInput, _playerData.MoveSpeed);
    }

    public override void OnMoveEnd()
    {
        _baseFSM.SetState(Player.MovementState.Stop);
    }

    public override void OnDash()
    {
        //bool canUseDash = CanUseDash();
        //if (canUseDash == false) return;

        //UseDash?.Invoke();
        //_baseFSM.SetState(Player.MovementState.Dash, _storedInput, "GoToDashState");
    }
}