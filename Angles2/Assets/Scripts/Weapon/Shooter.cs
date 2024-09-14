using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기서 IUpgradable를 재정의
// _weaponData를 증가시키는 방향으로 개발 진행

abstract public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    TrackComponent _trackComponent;

    protected WeaponData _weaponData;
    protected abstract BaseWeapon ReturnProjectileWeapon();

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (_shooterData._fireDelay > _waitFire) return;

        _waitFire = 0;

        BaseWeapon weapon = ReturnProjectileWeapon();
        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _shooterData._shootForce);
    }

    protected float _waitFire;
    List<ITarget> _targetDatas;

    protected ShooterData _shooterData;
    protected BaseFactory _weaponFactory;

    public override void ResetData(ShooterData shooterData)
    {
        _shooterData = shooterData;
        _trackComponent.Initialize(_shooterData._moveSpeed, _shooterData._followOffset, _shooterData._maxDistanceFromPlayer);
    }

    public override void Initialize(BaseFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
        _waitFire = 0;

        _trackComponent = GetComponent<TrackComponent>();

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

    private void Update()
    {
        ITarget target = ReturnCapturedTarget();
        if (target == null) return;

        Vector3 targetPos = target.ReturnPosition();
        Vector2 direction = (targetPos - transform.position).normalized;
        FireProjectile(direction);
    }
}