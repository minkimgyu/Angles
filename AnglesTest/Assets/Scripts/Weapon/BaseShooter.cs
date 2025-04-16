using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���⼭ IUpgradable�� ������
// CopyWeaponData�� ������Ű�� �������� ���� ����

abstract public class BaseShooter : BaseWeapon
{
    FollowComponent _followComponent;

    protected ShooterData _data;
    protected BaseFactory _weaponFactory;

    public override void ModifyData(ShooterDataModifier modifier)
    {
        modifier.Visit(_data);
        _detectingStrategy.InjectTargetTypes(_data.TargetTypes);
    }

    public override void InjectData(ShooterData shooterData)
    {
        _data = shooterData;
    }

    public override void Initialize(BaseFactory weaponFactory)
    {
        base.Initialize(weaponFactory);
        _weaponFactory = weaponFactory;
    }

    public override void InitializeStrategy()
    {
        base.InitializeStrategy();
        _followComponent = GetComponent<FollowComponent>();
        _followComponent.Initialize
        (
            _data.MoveSpeed,
            _data.FollowOffset,
            new Vector2(_data.FollowOffsetDirection.x, _data.FollowOffsetDirection.y),
            _data.MaxDistanceFromPlayer
        );

        TargetCaptureComponent targetCaptureComponent = GetComponentInChildren<TargetCaptureComponent>();
        _detectingStrategy = new TargetDetectingStrategy(targetCaptureComponent);
        _moveStrategy = new FollowingMoveStrategy(_followComponent);
    }
}