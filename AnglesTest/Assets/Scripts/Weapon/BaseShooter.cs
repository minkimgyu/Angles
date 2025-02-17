using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기서 IUpgradable를 재정의
// CopyWeaponData를 증가시키는 방향으로 개발 진행

abstract public class BaseShooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    FollowComponent _trackComponent;

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (_data.FireDelay > _waitFire) return;

        _waitFire = 0;

        BaseWeapon weapon = CreateProjectileWeapon();
        weapon.ResetPosition(transform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _data.ShootForce);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.ShooterFire, transform.position);
    }

    protected abstract BaseWeapon CreateProjectileWeapon();

    protected float _waitFire;
    List<ITarget> _targetDatas;

    protected ShooterData _data;
    protected BaseFactory _weaponFactory;

    public override void ModifyData(ShooterDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void ResetData(ShooterData shooterData)
    {
        _data = shooterData;
    }

    public override void Initialize(BaseFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
        _waitFire = 0;

        _trackComponent = GetComponent<FollowComponent>();
        _trackComponent.Initialize(
            _data.MoveSpeed,
            _data.FollowOffset,
            new Vector2(_data.FollowOffsetDirection.x, _data.FollowOffsetDirection.y),
            _data.MaxDistanceFromPlayer);

        _lifetimeComponent = new NoLifetimeComponent();
        _sizeModifyComponent = new NoSizeModifyComponent();

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

            bool isTarget = _targetDatas[i].IsTarget(_data.DamageableData._targetType);
            if (isTarget == false) continue;

            capturedTarget = _targetDatas[i];
            break;
        }

        return capturedTarget;
    }

    protected override void Update()
    {
        base.Update();
        ITarget target = ReturnCapturedTarget();
        if (target == null) return;

        Vector3 targetPos = target.GetPosition();
        Vector2 direction = (targetPos - transform.position).normalized;
        FireProjectile(direction);
    }
}