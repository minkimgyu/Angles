using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기서 IUpgradable를 재정의
// CopyWeaponData를 증가시키는 방향으로 개발 진행

abstract public class BaseShooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    FollowComponent _followComponent;

    protected ShooterData _data;
    protected BaseFactory _weaponFactory;

    public override void ModifyData(ShooterDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }

    public override void InjectData(ShooterData shooterData)
    {
        _data = shooterData;
    }

    public override void Initialize(BaseFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;

        _followComponent = GetComponent<FollowComponent>();
        _followComponent.Initialize(
            _data.MoveSpeed,
            _data.FollowOffset,
            new Vector2(_data.FollowOffsetDirection.x, _data.FollowOffsetDirection.y),
            _data.MaxDistanceFromPlayer);

        _lifeTimeStrategy = new NoLifetimeStrategy();
        _sizeStrategy = new NoSizeStrategy();

        _targetCaptureComponent = GetComponentInChildren<TargetCaptureComponent>();
        _targetCaptureComponent.Initialize(OnEnter, OnExit);
    }

    void OnEnter(ITarget target)
    {
        _attackStrategy.OnTargetEnter(target);
    }

    void OnExit(ITarget target)
    {
        _attackStrategy.OnTargetExit(target);
    }

    public override void ResetFollower(IFollowable followable)
    {
        _followComponent.ResetFollower(followable);
    }

    protected override void Update()
    {
        base.Update();
        _attackStrategy.OnUpdate();
    }
}