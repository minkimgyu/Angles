using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Skill.Strategy
{
    public interface IRoutineStrategy
    {
        void ChangeDelay(float delay) { }

        void OnUpdate() { }
        void OnDamaged(float ratio) { }
    }

    public class NoDelayStrategy : IRoutineStrategy
    {
    }

    public class DelayOnDamageStrategy : IRoutineStrategy
    {
        float _delay;
        float _hpRatioToInvoke;

        Timer _delayTimer;

        Action OnStart;
        Action OnCompleted;

        public DelayOnDamageStrategy(
            float delay,
            float hpRatioToInvoke,
            Action OnStart,
            Action OnCompleted)
        {
            _delayTimer = new Timer();
            _delay = delay;
            _hpRatioToInvoke = hpRatioToInvoke;
            this.OnStart = OnStart;
            this.OnCompleted = OnCompleted;
        }

        public void ChangeDelay(float delay) 
        {
            _delay = delay;
            _delayTimer.Reset();
            _delayTimer.Start(delay);
        }

        public void OnDamaged(float ratio)
        {
            if (ratio > _hpRatioToInvoke) return;
            if (_delayTimer.CurrentState != Timer.State.Ready) return;

            OnStart?.Invoke();
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

    public class DelayRoutineStrategy : IRoutineStrategy
    {
        float _delay;
        Timer _delayTimer;

        Action OnStart;
        Action OnCompleted;
        Func<bool> CanStartRoutine;

        public DelayRoutineStrategy(
            float delay,
            Func<bool> CanStartRoutine,
            Action OnCompleted)
        {
            _delayTimer = new Timer();
            _delay = delay;
            this.CanStartRoutine = CanStartRoutine;
            this.OnCompleted = OnCompleted;
        }

        public DelayRoutineStrategy(
           float delay,
           Func<bool> CanStartRoutine,
           Action OnStart,
           Action OnCompleted)
        {
            _delayTimer = new Timer();
            _delay = delay;
            this.OnStart = OnStart;
            this.CanStartRoutine = CanStartRoutine;
            this.OnCompleted = OnCompleted;
        }

        public void ChangeDelay(float delay)
        {
            _delay = delay;
            _delayTimer.Reset();
            _delayTimer.Start(delay);
        }

        public void OnUpdate()
        {
            switch (_delayTimer.CurrentState)
            {
                case Timer.State.Ready:
                    if (CanStartRoutine == null || CanStartRoutine() == false) return;

                    OnStart?.Invoke();
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

    public class WaveDelayRoutineStrategy : IRoutineStrategy
    {
        int _waveCount;
        int _maxWaveCount;

        float _waveDelay;
        Timer _waveTimer;

        float _delay;
        Timer _delayTimer;
        Func<bool> CanStartRoutine;

        Action OnStart;
        Action<int> OnCompleted;

        public WaveDelayRoutineStrategy(
            float delay,
            int waveCount,
            float waveDelay,
            Func<bool> CanStartRoutine,
            Action OnStart,
            Action<int> OnCompleted)
        {
            _delayTimer = new Timer();
            _waveTimer = new Timer();
            _delay = delay;
            _waveCount = 0;
            _maxWaveCount = waveCount;
            _waveDelay = waveDelay;

            this.CanStartRoutine = CanStartRoutine;
            this.OnStart = OnStart;
            this.OnCompleted = OnCompleted;
        }

        public void ChangeDelay(float delay)
        {
            _delay = delay;
            _delayTimer.Reset();
            _delayTimer.Start(delay);
        }

        public void OnUpdate()
        {
            switch (_delayTimer.CurrentState)
            {
                case Timer.State.Ready:
                    if (CanStartRoutine == null || CanStartRoutine() == false) return;
                    OnStart?.Invoke();
                    _delayTimer.Start(_delay);
                    break;
                case Timer.State.Finish:

                    if (_waveTimer.CurrentState == Timer.State.Ready || _waveTimer.CurrentState == Timer.State.Finish)
                    {
                        _waveTimer.Reset();
                        _waveTimer.Start(_waveDelay);
                        _waveCount++;

                        OnCompleted?.Invoke(_waveCount);

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
}