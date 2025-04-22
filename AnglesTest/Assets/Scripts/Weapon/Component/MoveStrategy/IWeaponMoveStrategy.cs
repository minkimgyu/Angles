using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

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

    void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath) { }
    void InjectFollower(IFollowable followable) { }
}

public class NoMoveStrategy : IWeaponMoveStrategy
{
}

public class TrackingMoveStrategy : IWeaponMoveStrategy
{
    float _moveSpeed;
    MoveComponent _moveComponent;
    TrackComponent _trackComponent;

    public TrackingMoveStrategy(
        float moveSpeed,
        MoveComponent moveComponent,
        TrackComponent trackComponent)
    {
        _moveSpeed = moveSpeed;

        _moveComponent = moveComponent;
        _trackComponent = trackComponent;
    }

    public void InjectTarget(ITarget target)
    {
        _trackComponent.InjectTarget(target);
    }

    public void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath)
    {
        _trackComponent.InjectPathFindEvent(FindPath);
    }

    public void OnUpdate()
    {
        _trackComponent.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _moveComponent.Move(_trackComponent.MoveDirection, _moveSpeed);
    }
}

public class MoveBetweenPointsStrategy : IWeaponMoveStrategy
{
    List<Transform> _movePoints;
    Transform _myTransform;
    MoveComponent _moveComponent;
    int _moveIndex;
    float _closeDistance = 0.5f;
    Vector2 _moveDirection;
    float _speed;

    public MoveBetweenPointsStrategy
    (
        List<Transform> points,
        Transform myTransform,
        MoveComponent moveComponent,
        float speed) : base()
    {
        _movePoints = points;
        _myTransform = myTransform;
        _moveComponent = moveComponent;
        _moveIndex = 0;
        _closeDistance = 0.5f;
        _speed = speed;
    }

    public void OnUpdate()
    {
        Vector2 nextMovePos = _movePoints[_moveIndex].position;
        _moveDirection = (nextMovePos - (Vector2)_myTransform.position).normalized;

        // 특정 점과 가까워지면 index +1 해주기
        bool nowCloseToNextPoint = Vector2.Distance(_myTransform.position, nextMovePos) < _closeDistance;
        if (nowCloseToNextPoint)
        {
            _moveIndex++;
            if (_movePoints.Count == _moveIndex) _moveIndex = 0;
        }
    }

    public void OnFixedUpdate()
    {
        _moveComponent.Move(_moveDirection, _speed);
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

    public RefractableProjectileMoveStrategy
    (
        MoveComponent moveComponent,
        Transform myTransform
    ) : base(moveComponent, myTransform) { }

    public override void OnFixedUpdate()
    {
        _moveComponent.RotateDirection(_rotationSpeed);
    }
}