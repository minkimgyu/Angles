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
    float _fireDelay;

    float _offsetToFollower;

    Vector2 _followOffset;

    public override void Initialize(float damage, float moveSpeed, float fireMaxDelay, float offsetToFollower)
    {
        _moveSpeed = moveSpeed;
        _fireMaxDelay = fireMaxDelay;
        _fireDelay = 0;

        _offsetToFollower = offsetToFollower;
        _targetCaptureComponent = GetComponent<TargetCaptureComponent>();
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

        List<ITarget> targets = _targetCaptureComponent.ReturnTargets();
        for (int i = 0; i < targets.Count; i++)
        {
            ITarget.Type targetType = targets[i].ReturnTargetType();
            bool isOtherSide = _targetTypes.Contains(targetType);
            if (isOtherSide == false) continue;

            capturedTarget = targets[i];
            break;
        }

        return capturedTarget;
    }

    void FireProjectile(Vector2 direction)
    {
        _fireDelay += Time.deltaTime;
        if (_fireMaxDelay > _fireDelay) return;

        _fireDelay = 0;
        BaseWeapon weapon = WeaponFactory.Create(Name.Shooter);
        weapon.ResetPosition(transform.position);
        weapon.ResetDamageableTypes(_targetTypes);

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
