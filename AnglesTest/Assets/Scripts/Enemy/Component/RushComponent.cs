using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

// 전략 패턴으로 연계해도 좋을 듯

public class RushComponent : IMoveStrategy
{
    public enum State
    {
        Stop,
        Rush
    }

    float _moveSpeed;

    Timer _stateTimer;
    float _stopDuration = 3f;
    float _rushDuration = 5f;

    Transform _myTransform;
    MoveComponent _moveComponent;

    Action<Collision2D> OnCollisionRequested;

    public RushComponent(
        MoveComponent moveComponent,
        Transform myTransform,
        float stopDuration,
        float rushDuration,
        float moveSpeed,
        Action<Collision2D> OnCollisionRequested)
    {
        _moveSpeed = moveSpeed;
        _stopDuration = stopDuration;
        _rushDuration = rushDuration;

        _myTransform = myTransform;
        _moveComponent = moveComponent;

        this.OnCollisionRequested = OnCollisionRequested;

        _state = State.Rush;
        _stateTimer = new Timer();
        _stateTimer.Start(_rushDuration);
    }

    State _state;

    Vector2 GetRandomDirection()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        return new Vector2(x, y);
    }

    public void OnUpdate()
    {
        switch (_state)
        {
            case State.Stop:
                if (_stateTimer.CurrentState == Timer.State.Running) return;
                if (_stateTimer.CurrentState == Timer.State.Finish)
                {
                    _state = State.Rush;
                    _stateTimer.Reset();
                    return;
                }
                break;
            case State.Rush:

                if (_stateTimer.CurrentState == Timer.State.Running) return;
                if(_stateTimer.CurrentState == Timer.State.Finish)
                {
                    _state = State.Stop;
                    _stateTimer.Reset();
                    return;
                }

                Vector2 dir = GetRandomDirection();
                _moveComponent.AddForce(dir, _moveSpeed);
                _stateTimer.Start(_rushDuration);
                break;
            default:
                break;
        }
    }

    public virtual void OnCollision(Collision2D collision)
    {
        switch (_state)
        {
            case State.Rush:
                OnCollisionRequested?.Invoke(collision);
                _moveComponent.ResetVelocity(); // 속도 리셋 후 다시 이동

                Vector2 dir = collision.contacts[0].normal;
                _moveComponent.AddForce(dir, _moveSpeed);
                break;
            default:
                break;
        }
    }

    public void OnFixedUpdate()
    {
        switch (_state)
        {
            case State.Stop:
                _moveComponent.Stop();
                break;
            default:
                break;
        }
    }
}
