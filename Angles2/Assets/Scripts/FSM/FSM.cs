using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    Dictionary<T, BaseState> _states;

    BaseState _currentState;
    BaseState _previousState;

    public void Inintialize(Dictionary<T, BaseState> states, T startState)
    {
        _currentState = null;
        _previousState = null;

        _states = states;
        SetState(startState);
    }

    public void OnUpdate() => _currentState.OnStateUpdate();
    public void OnFixedUpdate() => _currentState.OnFixedUpdate();

    public void OnMoveStart() => _currentState.OnMoveStart();
    public void OnMove(Vector2 direction) => _currentState.OnMove(direction);
    public void OnMoveEnd() => _currentState.OnMoveEnd();

    public void OnChargeStart() => _currentState.OnChargeStart();
    public void OnCharge(Vector2 direction) => _currentState.OnCharge(direction);
    public void OnChargeEnd() => _currentState.OnChargeEnd();

    public void OnDash() => _currentState.OnDash();

    public void OnCollisionEnter(Collision2D collision) => _currentState.OnCollisionEnter(collision);


    public bool SetState(T stateName)
    {
        return ChangeState(_states[stateName]);
    }

    public bool SetState(T stateName, Vector2 direction, string message)
    {
        return ChangeState(_states[stateName], direction, message);
    }

    public bool RevertToPreviousState()
    {
        return ChangeState(_previousState);
    }

    public bool RevertToPreviousState(Vector2 vec2, string message)
    {
        return ChangeState(_previousState, vec2, message);
    }

    bool ChangeState(BaseState state)
    {
        if (_states.ContainsValue(state) == false) return false;

        if (_currentState == state) // 같은 State로 전환하지 못하게 막기
        {
            return false;
        }

        if (_currentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출
            _currentState.OnStateExit();

        _previousState = _currentState;

        _currentState = state;


        if (_currentState != null) //새 상태의 Enter를 호출한다.
        {
            _currentState.OnStateEnter();
        }

        return true;
    }

    bool ChangeState(BaseState state, Vector2 direction, string message)
    {
        if (_states.ContainsValue(state) == false) return false;

        if (_currentState == state) // 같은 State로 전환하지 못하게 막기
        {
            return false;
        }

        if (_currentState != null) //상태가 바뀌기 전에, 이전 상태의 Exit를 호출
            _currentState.OnStateExit();

        _previousState = _currentState;

        _currentState = state;


        if (_currentState != null) //새 상태의 Enter를 호출한다.
        {
            _currentState.OnStateEnter(direction, message);
        }

        return true;
    }
}
