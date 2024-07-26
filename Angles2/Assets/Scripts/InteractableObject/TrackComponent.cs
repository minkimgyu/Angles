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

    public void ResetFollower(IFollowable followable)
    {
        _followableTarget = followable;
    }

    public void Initialize(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        _followOffset = 0;
        Initialize();
    }

    public void Initialize(float moveSpeed, float followOffset)
    {
        _moveSpeed = moveSpeed;
        _followOffset = followOffset;
        Initialize();
    }

    private void FixedUpdate()
    {
        if (_followableTarget == null) return;

        Move(_followPos);
    }

    private void Update()
    {
        if (_followableTarget == null) return;

        Vector2 pos = _followableTarget.ReturnPosition();
        Vector2 foward = _followableTarget.ReturnFowardDirection();

        Vector2 offset = -foward * _followOffset;
        _followPos = Vector2.Lerp(transform.position, pos + offset, Time.deltaTime * _moveSpeed);
    }
}
