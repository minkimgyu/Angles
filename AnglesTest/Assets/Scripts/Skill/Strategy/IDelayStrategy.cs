using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDelayStrategy
{
    void OnUpdate() { }
    void OnDamaged(float ratio) { }
}

public class NoDelayStrategy : IDelayStrategy
{
}

public class DelayOnDamageStrategy : IDelayStrategy
{
    float _delay;
    float _hpRatioToInvoke;

    Timer _delayTimer;
    Action OnCompleted;

    public DelayOnDamageStrategy(float delay, float hpRatioToInvoke, Action OnCompleted)
    {
        _delayTimer = new Timer();
        _delay = delay;
        _hpRatioToInvoke = hpRatioToInvoke;
        this.OnCompleted = OnCompleted;
    }

    public void OnDamaged(float ratio) 
    {
        if (ratio > _hpRatioToInvoke) return;
        if (_delayTimer.CurrentState != Timer.State.Ready) return;

        _delayTimer.Start(_delay);
    }

    public void OnUpdate()
    {
        if (_delayTimer.CurrentState == Timer.State.Finish)
        {
            OnCompleted?.Invoke();
            _delayTimer.Reset();
        }
    }
}

public class DelayRoutineStrategy : IDelayStrategy
{
    float _delay;
    Timer _delayTimer;
    Action OnCompleted;

    public DelayRoutineStrategy(float delay, Action OnCompleted)
    {
        _delayTimer = new Timer();
        _delay = delay;
        this.OnCompleted = OnCompleted;
    }

    public void OnUpdate() 
    {
        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                _delayTimer.Start(_delay);
                break;
            case Timer.State.Finish:
                OnCompleted?.Invoke();
                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }
}

public class WaveDelayRoutineStrategy : IDelayStrategy
{
    int _waveCount;
    int _maxWaveCount;

    float _waveDelay;
    Timer _waveTimer;

    float _delay;
    Timer _delayTimer;
    Action OnCompleted;

    public WaveDelayRoutineStrategy(float delay, int waveCount, float waveDelay, Action OnCompleted)
    {
        _delayTimer = new Timer();
        _delay = delay;
        _waveCount = 0;
        _maxWaveCount = waveCount;
        _waveDelay = waveDelay;
        this.OnCompleted = OnCompleted;
    }

    public void OnUpdate()
    {
        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                _delayTimer.Start(_delay);
                break;
            case Timer.State.Finish:

                if (_waveTimer.CurrentState == Timer.State.Ready || _waveTimer.CurrentState == Timer.State.Finish)
                {
                    OnCompleted?.Invoke();
                    _waveTimer.Reset();
                    _waveTimer.Start(_waveDelay);
                    _waveCount++;

                    Debug.Log(_waveCount);
                    if (_waveCount == _maxWaveCount)
                    {
                        _waveCount = 0;
                        _waveTimer.Reset();
                        _delayTimer.Reset();
                    }
                }
                break;
            default:
                break;
        }
    }
}