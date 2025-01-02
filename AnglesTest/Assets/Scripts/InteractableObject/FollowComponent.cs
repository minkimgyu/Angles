using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// BaseTrackComponent
// EventTrackComponent
// OffsetTrackComponent
// --> �и��ؼ� �ٸ��� ����ϱ�

public class FollowComponent : MoveComponent
{
    IFollowable _followableTarget;
    float _moveSpeed = 8f;

    Vector2 _followPos;
    float _followOffset;
    float _maxDistanceFromPlayer;

    const float _maxDictance = 1000000;

    Vector2 _followOffsetDirection;

    public void ResetFollower(IFollowable followable)
    {
        _followableTarget = followable;
    }

    public void Initialize(float moveSpeed)
    {
        _moveSpeed = moveSpeed;

        _followOffset = 0;
        _followOffsetDirection = Vector2.zero;

        _maxDistanceFromPlayer = _maxDictance;
        Initialize();
    }

    public void Initialize(float moveSpeed, float followOffset, Vector2 followOffsetDirection, float maxDistanceFromPlayer)
    {
        _moveSpeed = moveSpeed;

        _followOffset = followOffset;
        _followOffsetDirection = followOffsetDirection.normalized;

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

        Vector2 pos = _followableTarget.GetPosition();
        Vector2 offset = -_followOffsetDirection * _followOffset;

        if (Vector2.Distance(pos, transform.position) > _maxDistanceFromPlayer) // �÷��̾��� �Ÿ��� �� �ִٸ�
        {
            _followPos = pos + offset;
        }
        else
        {
            _followPos = Vector2.Lerp(transform.position, pos + offset, Time.deltaTime * _moveSpeed);
        }
    }
}
