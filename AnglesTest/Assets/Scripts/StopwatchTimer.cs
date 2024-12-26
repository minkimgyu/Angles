using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchTimer
{
    public enum State
    {
        Ready,
        Running,
        Finish
    }

    float _duration;

    State _state;

    public State CurrentState{ get { return _state; } }
    public float Duration { get { return _duration; } }

    public StopwatchTimer()
    {
        _state = State.Ready;
        _duration = 0;
    }

    public void OnUpdate()
    {
        if(_state == State.Running) _duration += Time.deltaTime;
    }

    public void Stop()
    {
        if (_state != State.Running) return;
        _state = State.Finish;
    }

    public void Start()
    {
        if (_state != State.Ready) return;
        _state = State.Running;
        _duration = 0;
    }

    // Ÿ�̸Ӹ� ó������ �ʱ�ȭ���ش�.
    public void Reset()
    {
        if (_state == State.Ready) return;
        _state = State.Ready;
        _duration = 0;
    }
}
