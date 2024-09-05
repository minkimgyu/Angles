using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    TrackComponent _trackComponent;

    List<ShooterUpgradableData> _upgradableDatas;
    ShooterUpgradableData UpgradableData { get { return _upgradableDatas[_upgradePoint]; } }

    protected BaseWeaponData _weaponData;

    protected abstract BaseWeapon ReturnProjectileWeapon();

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (UpgradableData.FireDelay > _waitFire) return;

        _waitFire = 0;

        BaseWeapon weapon = ReturnProjectileWeapon();
        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, UpgradableData.ShootForce);
    }

    protected float _moveSpeed;
    protected float _followOffset;
    protected float _waitFire;
    protected float _maxDistanceFromPlayer;
    protected Name _fireWeaponName;

    List<ITarget> _targetDatas;

    protected BaseFactory _weaponFactory;

    public override void ResetData(ShooterData data)
    {
        _upgradableDatas = data._upgradableDatas;

        _weaponData = data._fireWeaponData;
        _moveSpeed = data._moveSpeed;
        _followOffset = data._followOffset;
        _maxDistanceFromPlayer = data._maxDistanceFromPlayer;
        _fireWeaponName = data._fireWeaponName;

        _trackComponent.Initialize(_moveSpeed, _followOffset, _maxDistanceFromPlayer);
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
