using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseFSM<T>
{
    Dictionary<T, BaseState<T>> _states;

    protected BaseState<T> _currentState;
    protected BaseState<T> _previousState;

    public void Inintialize(Dictionary<T, BaseState<T>> states, T startState)
    {
        _currentState = null;
        _previousState = null;

        _states = states;
        SetState(startState);
    }

    public bool SetState(T stateName)
    {
        return ChangeState(_states[stateName]);
    }

    public bool SetState(T stateName, Vector2 direction, string message)
    {
        return ChangeState(_states[stateName], direction, message);
    }

    public bool SetState(T stateName, Vector2 direction, float ratio, string message)
    {
        return ChangeState(_states[stateName], direction, ratio, message);
    }

    public bool RevertToPreviousState()
    {
        return ChangeState(_previousState);
    }

    public bool RevertToPreviousState(Vector2 vec2, string message)
    {
        return ChangeState(_previousState, vec2, message);
    }

    public bool RevertToPreviousState(Vector2 vec2, float ratio, string message)
    {
        return ChangeState(_previousState, vec2, ratio, message);
    }

    bool ChangeState(BaseState<T> state)
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

    bool ChangeState(BaseState<T> state, Vector2 direction, string message)
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

    bool ChangeState(BaseState<T> state, Vector2 direction, float ratio, string message)
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
            _currentState.OnStateEnter(direction, ratio, message);
        }

        return true;
    }
}

public class FSM<T> : BaseFSM<T>
{
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
}
