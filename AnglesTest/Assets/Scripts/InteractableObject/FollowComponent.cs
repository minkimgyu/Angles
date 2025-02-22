using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// BaseTrackComponent
// EventTrackComponent
// OffsetTrackComponent
// --> 분리해서 다르게 사용하기

[RequireComponent(typeof(MoveComponent))]
public class FollowComponent : MonoBehaviour
{
    IFollowable _followableTarget;
    float _moveSpeed = 8f;

    Vector2 _followPos;
    float _followOffset;
    float _maxDistanceFromPlayer;

    const float _maxDictance = 1000000;
    const float _maxMoveSpeed = 1000000;

    Vector2 _followOffsetDirection;

    public void InjectFollower(IFollowable followable)
    {
        _followableTarget = followable;
    }

    MoveComponent _moveComponent;

    public void Initialize()
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _moveSpeed = _maxMoveSpeed;
        _followOffset = 0;
        _followOffsetDirection = Vector2.zero;
        _maxDistanceFromPlayer = _maxDictance;
    }

    public void Initialize(float moveSpeed)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _moveSpeed = moveSpeed;
        _followOffset = 0;
        _followOffsetDirection = Vector2.zero;
        _maxDistanceFromPlayer = _maxDictance;
    }

    public void Initialize(float moveSpeed, float followOffset, Vector2 followOffsetDirection, float maxDistanceFromPlayer)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        _followOffsetDirection = followOffsetDirection.normalized;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
    }

    public void FreezePosition(bool freeze)
    {
        _moveComponent.FreezePosition(freeze);
    }

    public void OnFixedUpdate()
    {
        if ((_followableTarget as UnityEngine.Object) == null) return;

        _moveComponent.Move(_followPos);
    }

    public void OnUpdate()
    {
        if ((_followableTarget as UnityEngine.Object) == null) return;

        Vector2 pos = _followableTarget.GetPosition();
        Vector2 offset = -_followOffsetDirection * _followOffset;

        if (Vector2.Distance(pos, transform.position) > _maxDistanceFromPlayer) // 플레이어의 거리가 더 멀다면
        {
            _followPos = pos + offset;
        }
        else
        {
            _followPos = Vector2.Lerp(transform.position, pos + offset, Time.deltaTime * _moveSpeed);
        }
    }
}
