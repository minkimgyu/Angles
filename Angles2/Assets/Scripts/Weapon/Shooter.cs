using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    TrackComponent _trackComponent;

    float _moveSpeed;
    float _shootForce;
    float _fireDelay;
    float _followOffset;
    float _waitFire;
    float _maxDistanceFromPlayer;
    Name _fireWeaponName;

    List<ITarget> _targetDatas;

    System.Func<Name, BaseWeapon> SpawnWeapon;

    public override void Initialize(ShooterData data, System.Func<Name, BaseWeapon> SpawnWeapon)
    {
        this.SpawnWeapon = SpawnWeapon;

        _moveSpeed = data._moveSpeed;
        _shootForce = data._shootForce;
        _fireDelay = data._fireDelay;
        _followOffset = data._followOffset;
        _maxDistanceFromPlayer = data._maxDistanceFromPlayer;
        _fireWeaponName = data._fireWeaponName;

        _waitFire = 0;

        _trackComponent = GetComponent<TrackComponent>();
        _trackComponent.Initialize(_moveSpeed, _followOffset, _maxDistanceFromPlayer);

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
        _trackComponent.ResetFollower(followable);
    }

    ITarget ReturnCapturedTarget()
    {
        ITarget capturedTarget = null;

        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            if ((_targetDatas[i] as UnityEngine.Object) == null) continue;

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
        BaseWeapon weapon = SpawnWeapon?.Invoke(_fireWeaponName);
        weapon.ResetPosition(transform.position);
        weapon.ResetTargetTypes(_targetTypes);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _shootForce);
    }

    private void Update()
    {
        ITarget target = ReturnCapturedTarget();
        if (target == null) return;

        Vector3 targetPos = target.ReturnPosition();
        Vector2 direction = (targetPos - transform.position).normalized;
        FireProjectile(direction);
    }
}
