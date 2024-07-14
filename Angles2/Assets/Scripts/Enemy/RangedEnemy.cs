using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    public enum FollowState
    {
        Stop,
        Follow,
    }

    FollowState _movementState;

    protected float _stopDistance;
    protected float _gap;

    float ReturnDistanceBetweenTarget()
    {
        Vector3 targetPos = _followTarget.ReturnPosition();
        return Vector2.Distance(transform.position, targetPos);
    }

    protected override void Update()
    {
        base.Update();
        float diatance;

        switch (_movementState)
        {
            case FollowState.Stop:
                diatance = ReturnDistanceBetweenTarget();
                if (diatance <= _stopDistance + _gap) break;

                _moveComponent.FreezeRotation(false);
                _movementState = FollowState.Follow;
                break;
            case FollowState.Follow:
                diatance = ReturnDistanceBetweenTarget();
                if (diatance < _stopDistance)
                {
                    _moveComponent.FreezeRotation(true);
                    _movementState = FollowState.Stop;
                    break;
                }

                MoveToDirection();
                break;
            default:
                break;
        }

        ResetDirection();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (_movementState)
        {
            case FollowState.Stop:
                _moveComponent.Stop();
                break;
            case FollowState.Follow:
                MoveToDirection();
                break;
            default:
                break;
        }
    }
}
