using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    Transform _follower;

    float _moveSpeed;
    float _fireMaxDelay;
    float _offsetToFollower;

    float _fireDelay;
    Vector2 _followOffset;
    List<ITarget> _targetDatas;

    public override void Initialize(ShooterData data)
    {
        _moveSpeed = data._moveSpeed;
        _fireMaxDelay = data._fireMaxDelay;
        _offsetToFollower = data._offsetToFollower;

        _fireDelay = 0;
        _followOffset = Vector2.zero;
        _targetDatas = new List<ITarget>();
        _targetCaptureComponent = GetComponent<TargetCaptureComponent>();
    }

    void OnEnter(ITarget target)
    {
        _targetDatas.Add(target);
    }

    void OnExit(ITarget target)
    {
        _targetDatas.Remove(target);
    }

    public override void ResetFollower(Transform follower)
    {
        _follower = follower;
    }

    private void MoveToFollower()
    {
        _followOffset = -_follower.right * _offsetToFollower;
        transform.position = Vector3.Lerp(transform.position, _follower.position, Time.deltaTime * _moveSpeed);
    }

    ITarget ReturnCapturedTarget()
    {
        ITarget capturedTarget = null;

        for (int i = 0; i < _targetDatas.Count; i++)
        {
            bool isOtherSide = _targetTypes.Contains(_targetDatas[i].ReturnTargetType());
            if (isOtherSide == false) continue;

            capturedTarget = _targetDatas[i];
            break;
        }

        return capturedTarget;
    }

    void FireProjectile(Vector2 direction)
    {
        _fireDelay += Time.deltaTime;
        if (_fireMaxDelay > _fireDelay) return;

        _fireDelay = 0;
        BaseWeapon weapon = WeaponFactory.Create(Name.Bullet);
        weapon.ResetPosition(transform.position);
        weapon.ResetTargetTypes(_targetTypes);

        IProjectile projectile = weapon.GetComponent<IProjectile>();
        if (projectile == null) return;

        projectile.Shoot(direction);
    }

    private void Update()
    {
        ITarget target = ReturnCapturedTarget();

        Vector3 targetPos = target.ReturnPosition();
        Vector2 direction = targetPos - transform.position;

        FireProjectile(direction);
        MoveToFollower();
    }
}
