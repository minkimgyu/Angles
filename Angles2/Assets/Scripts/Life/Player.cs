using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Life
{
    public enum MovementState
    {
        Stop,
        Move,
        Dash,
    }

    public enum ActionState
    {
        Ready,
        Charge,
        Shoot,
        Reflect
    }

    FSM<MovementState> _movementFSM;
    FSM<ActionState> _actionFSM;

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _dashSpeed = 10f;
    [SerializeField] float _dashDuration = 1.5f;

    [SerializeField] float _minShootValue = 0.6f;
    [SerializeField] float _shootSpeed = 15f;
    [SerializeField] float _shootDuration = 1.5f;

    [SerializeField] int _maxDashCount = 3;
    [SerializeField] int _currentDashCount = 3;

    [SerializeField] int _dashUseCount = 1;

    float _dashFillDuration = 0;
    [SerializeField] float _maxDashFillDuration = 5f;

    float DashRatio { get { return _dashFillDuration / _maxDashFillDuration; } }
    float TotalDashRatio { get { return _currentDashCount + DashRatio; } }

    MoveComponent _moveComponent;
    DirectionComponent _directionComponent;

    void SetState(MovementState state) => _movementFSM.SetState(state);
    void SetState(MovementState state, Vector2 vec2, string message) => _movementFSM.SetState(state, vec2, message);

    void RevertToPreviousState(Vector2 vec2, string message) => _actionFSM.RevertToPreviousState(vec2, message);

    void SetState(ActionState state) => _actionFSM.SetState(state);
    void SetState(ActionState state, Vector2 vec2, string message) => _actionFSM.SetState(state, vec2, message);

    Action<float> UpdateViewer;

    private void Start()
    {
        DashUIController dashUIController = FindObjectOfType<DashUIController>();
        dashUIController.Initialize(_maxDashCount);

        UpdateViewer = dashUIController.UpdateViewer;

        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _directionComponent = GetComponent<DirectionComponent>();

        Dictionary<MovementState, BaseState> movementStates = new Dictionary<MovementState, BaseState>();
        movementStates.Add(MovementState.Stop, new StopState(SetState, _moveComponent.Stop));

        movementStates.Add(MovementState.Move, 
            new MoveState(_moveSpeed, CanUseDash, UseDash, _moveComponent.Move, SetState, SetState));

        movementStates.Add(MovementState.Dash, 
            new DashState(_dashSpeed, _dashDuration, SetState, _moveComponent.AddForce));

        _movementFSM = new FSM<MovementState>();
        _movementFSM.Inintialize(movementStates, MovementState.Stop);


        Dictionary<ActionState, BaseState> actionStates = new Dictionary<ActionState, BaseState>();
        actionStates.Add(ActionState.Ready, new ReadyState(SetState));

        actionStates.Add(ActionState.Charge, new ChargeState(_minShootValue, 
            (value) => { _directionComponent.OnOffDirectionSprite(value); _moveComponent.ApplyDirection = !value; }, 
            _moveComponent.FaceDirection, SetState, SetState));

        actionStates.Add(ActionState.Shoot, 
            new ShootState(_shootSpeed, _shootDuration, SetState, SetState, _moveComponent.AddForce));

        actionStates.Add(ActionState.Reflect, new ReflectState(transform, RevertToPreviousState));

        _actionFSM = new FSM<ActionState>();
        _actionFSM.Inintialize(actionStates, ActionState.Ready);

        AddEvent();
    }

    protected override void OnDie()
    {
        ClearEvent();
    }

    bool CanUseDash() { return _currentDashCount >= _dashUseCount; }
    void UseDash() { _currentDashCount -= _dashUseCount; }

    void FillDashCount()
    {
        if (_maxDashCount == _currentDashCount) return;

        _dashFillDuration += Time.deltaTime;
        if(DashRatio >= 1)
        {
            _currentDashCount++;
            _dashFillDuration = 0;
        }
    }

    private void Update()
    {
        switch (_lifeState)
        {
            case LifeState.Alive:
                FillDashCount();
                UpdateViewer?.Invoke(TotalDashRatio);

                _movementFSM.OnUpdate();
                _actionFSM.OnUpdate();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _actionFSM.OnCollisionEnter(collision);
    }

    void ClearEvent()
    {
        InputManager.ClearEvent(InputManager.Side.Left);
        InputManager.ClearEvent(InputManager.Side.Right);
    }

    void AddEvent()
    {
        InputManager.AddEvent(
           InputManager.Side.Left,
           InputManager.Type.OnInputStart,
           _movementFSM.OnMoveStart
       );

        InputManager.AddEvent(
            InputManager.Side.Left,
            InputManager.Type.OnInput,
            (value) => { _movementFSM.OnMove(value); _actionFSM.OnMove(value); }
        );

        InputManager.AddEvent(
            InputManager.Side.Left,
            InputManager.Type.OnInputEnd,
            _movementFSM.OnMoveEnd
        );

        InputManager.AddEvent(
          InputManager.Side.Right,
          InputManager.Type.OnDoubleTab,
          _movementFSM.OnDash
      );


        InputManager.AddEvent(
           InputManager.Side.Right,
           InputManager.Type.OnInputStart,
           _actionFSM.OnChargeStart
       );

        InputManager.AddEvent(
           InputManager.Side.Right,
           InputManager.Type.OnInput,
          _actionFSM.OnCharge
        );

        InputManager.AddEvent(
            InputManager.Side.Right,
            InputManager.Type.OnInputEnd,
            _actionFSM.OnChargeEnd
        );
    }
}
