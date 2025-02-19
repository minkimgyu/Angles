using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//움직이는 방식

// 발사하는 방식
// 플레이어를 따라다니는 방식
// 적을 추적하는 방식
// 적에 부착되는 방식

public interface IWeaponMoveStrategy
{
    void OnCollisionEnter(Collision2D collision) { }
    void OnUpdate() { }
    void OnFixedUpdate() { }

    void Shoot(Vector3 direction, float force) { }
    void InjectTarget(ITarget target) { }

    void InjectFollower(IFollowable followable) { }
}

public class NoMoveStrategy : IWeaponMoveStrategy
{
}

public class TrackingMoveStrategy : IWeaponMoveStrategy
{
    ITarget _target;
    float _moveSpeed;
    Pathfinder _pathfinder;

    Transform _myTransform;
    MoveComponent _moveComponent;

    List<Vector2> _movePoints;
    int _index = 0;
    float _closeDistance = 0.5f;
    Vector2 _dir;

    Timer _pathfinderTimer;
    float _pathfindGap = 0.5f;


    public TrackingMoveStrategy(
        float moveSpeed,
        Pathfinder pathfinder,
        Transform myTransform,
        MoveComponent moveComponent)
    {
        _moveSpeed = moveSpeed;
        _pathfinder = pathfinder;
        _myTransform = myTransform;
        _moveComponent = moveComponent;

        _target = null;
        _movePoints = new List<Vector2>();
    }

    public void InjectTarget(ITarget target)
    {
        _target = target;
    }

    void ChangeDirection()
    {
        if (_movePoints == null) return;
        if (_index >= _movePoints.Count)
        {
            _dir = Vector2.zero;
            return;
        }

        Vector2 nextMovePos = _movePoints[_index];
        _dir = (nextMovePos - (Vector2)_myTransform.position).normalized;

        bool nowCloseToNextPoint = Vector2.Distance(_myTransform.position, nextMovePos) < _closeDistance;
        if (nowCloseToNextPoint) _index++;
    }

    public void OnUpdate()
    {
        if(_target as UnityEngine.Object == null) return;

        if (_pathfinderTimer.CurrentState == Timer.State.Finish)
        {
            _movePoints = _pathfinder.FindPath(_myTransform.position, _target.GetPosition(), BaseLife.Size.Small);
            _index = 0;

            _pathfinderTimer.Reset();
            _pathfinderTimer.Start(_pathfindGap);
        }

        ChangeDirection();
    }

    public void OnFixedUpdate()
    {
        _moveComponent.Move(_dir, _moveSpeed);
    }
}

public class FollowingMoveStrategy : IWeaponMoveStrategy
{
    FollowComponent _followComponent;

    public FollowingMoveStrategy(FollowComponent followComponent)
    {
        _followComponent = followComponent;
    }

    public void InjectFollower(IFollowable followable)
    {
        _followComponent.InjectFollower(followable);
    }

    public void OnUpdate()
    {
        _followComponent.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _followComponent.OnFixedUpdate();
    }
}

public class ProjectileMoveStrategy : IWeaponMoveStrategy
{
    protected float _force;
    protected Transform _myTransform;
    protected MoveComponent _moveComponent;

    public ProjectileMoveStrategy(
        MoveComponent moveComponent,
        Transform myTransform)
    {
        _moveComponent = moveComponent;
        _myTransform = myTransform;
    }

    public virtual void OnCollisionEnter(Collision2D collision) { }
    public virtual void OnFixedUpdate() { }

    public void Shoot(Vector3 direction, float force)
    {
        _myTransform.right = direction;
        _force = force;

        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _force);
    }
}

public class ReflectableProjectileMoveStrategy : ProjectileMoveStrategy
{
    public ReflectableProjectileMoveStrategy
    (
        MoveComponent moveComponent,
        Transform myTransform
    ) : base(moveComponent, myTransform) { }

    public override void OnCollisionEnter(Collision2D collision)
    {
        Debug.Log(collision.collider.name);

        Vector2 nomal = collision.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(_myTransform.right, nomal);
        Shoot(direction, _force);
    }
}

public class RefractableProjectileMoveStrategy : ProjectileMoveStrategy
{
    const float _rotationSpeed = 70f; // 회전 속도 (각속도, 도/초)
    const float _speedMultiplier = 1.5f; // 속도 배율

    public RefractableProjectileMoveStrategy
    (
        MoveComponent moveComponent,
        Transform myTransform
    ) : base(moveComponent, myTransform) { }

    public override void OnFixedUpdate()
    {
        _moveComponent.RotateDirection(_rotationSpeed, _speedMultiplier);
    }
}