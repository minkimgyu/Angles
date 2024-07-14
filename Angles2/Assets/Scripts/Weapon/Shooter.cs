using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    MoveComponent _moveComponent;

    IFollowable _followable;

    float _moveSpeed;
    float _shootForce;
    float _fireDelay;
    float _followOffset;
    float _waitFire;

    Vector2 _followPos;
    List<ITarget> _targetDatas;

    public override void Initialize(ShooterData data)
    {
        _moveSpeed = data._moveSpeed;
        _shootForce = data._shootForce;
        _fireDelay = data._fireDelay;
        _followOffset = data._followOffset;

        _waitFire = 0;
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _targetDatas = new List<ITarget>();
        _targetCaptureComponent = GetComponentInChildren<TargetCaptureComponent>();
        _targetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target)
    {
        _targetDatas.Add(target);
    }

    void OnExit(ITarget target)
    {
        _targetDatas.Remove(target);
    }

    public override void ResetFollower(IFollowable followable)
    {
        _followable = followable;
    }

    private void CalculateNextPos()
    {
        Vector2 pos = _followable.ReturnPosition();
        Vector2 foward = _followable.ReturnFowardDirection();

        Vector2 offset = -foward * _followOffset;
        _followPos = Vector2.Lerp(transform.position, pos + offset, Time.deltaTime * _moveSpeed);
    }

    private void MoveToFollower()
    {
        _moveComponent.Move(_followPos);
    }

    ITarget ReturnCapturedTarget()
    {
        ITarget capturedTarget = null;

        for (int i = 0; i < _targetDatas.Count; i++)
        {
            bool isTarget = _targetDatas[i].IsTarget(_targetTypes);
            if (isTarget == false) continue;

            capturedTarget = _targetDatas[i];
            break;
        }

        return capturedTarget;
    }

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (_fireDelay > _waitFire) return;

        _waitFire = 0;
        BaseWeapon weapon = WeaponFactory.Create(Name.Bullet);
        weapon.ResetPosition(transform.position);
        weapon.ResetTargetTypes(_targetTypes);

        IProjectile projectile = weapon.GetComponent<IProjectile>();
        if (projectile == null) return;

        projectile.Shoot(direction, _shootForce);
    }

    private void FixedUpdate()
    {
        MoveToFollower();
    }

    private void Update()
    {
        CalculateNextPos();

        ITarget target = ReturnCapturedTarget();
        if (target == null) return;

        Vector3 targetPos = target.ReturnPosition();
        Vector2 direction = targetPos - transform.position;

        FireProjectile(direction);
    }
}
