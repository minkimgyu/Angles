using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackComponent : MoveComponent
{
    IFollowable _followableTarget;
    float _moveSpeed = 8f;

    Vector2 _followPos;
    float _followOffset;
    float _maxDistanceFromPlayer;

    const float _maxDictance = 1000000;

    public void ResetFollower(IFollowable followable)
    {
        _followableTarget = followable;
    }

    public void Initialize(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        _followOffset = 0;
        _maxDistanceFromPlayer = _maxDictance;
        Initialize();
    }

    public void Initialize(float moveSpeed, float followOffset, float maxDistanceFromPlayer)
    {
        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        _maxDistanceFromPlayer = maxDistanceFromPlayer;
        Initialize();
    }

    private void FixedUpdate()
    {
        if ((_followableTarget as UnityEngine.Object) == null) return;

        Move(_followPos);
    }

    private void Update()
    {
        if ((_followableTarget as UnityEngine.Object) == null) return;

        Vector2 pos = _followableTarget.ReturnPosition();
        Vector2 foward = _followableTarget.ReturnFowardDirection();

        Vector2 offset = -foward * _followOffset;

        if (Vector2.Distance(pos, transform.position) > _maxDistanceFromPlayer)
        {
            _followPos = pos + offset;
        }
        else
        {
            _followPos = Vector2.Lerp(transform.position, pos + offset, Time.deltaTime * _moveSpeed);
        }
    }
}
