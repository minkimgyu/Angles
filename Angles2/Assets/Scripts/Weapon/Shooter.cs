using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여기서 IUpgradable를 재정의
// _weaponData를 증가시키는 방향으로 개발 진행

public class Shooter : BaseWeapon
{
    TargetCaptureComponent _targetCaptureComponent;
    TrackComponent _trackComponent;

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (_data._fireDelay > _waitFire) return;

        _waitFire = 0;

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(_data._damage));

        modifiers.Add(new WeaponTargetModifier(_data._targetTypes));
        modifiers.Add(new WeaponTotalDamageRatioModifier(_data._totalDamageRatio));

        BaseWeapon weapon = _weaponFactory.Create(_data._fireWeaponName);
        weapon.ModifyData(modifiers);
        weapon.ResetPosition(transform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _data._shootForce);
    }

    protected float _waitFire;
    List<ITarget> _targetDatas;

    protected ShooterData _data;
    protected BaseFactory _weaponFactory;

    public override void ModifyData(List<WeaponDataModifier> modifiers)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            _data = modifiers[i].Visit(_data);
        }
    }

    public override void ResetData(ShooterData shooterData)
    {
        _data = shooterData;
    }

    public override void Initialize(BaseFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
        _waitFire = 0;

        _trackComponent = GetComponent<TrackComponent>();
        _trackComponent.Initialize(_data._moveSpeed, _data._followOffset, _data._maxDistanceFromPlayer);

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

            bool isTarget = _targetDatas[i].IsTarget(_data._targetTypes);
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

        Vector3 targetPos = target.ReturnPosition();
        Vector2 direction = (targetPos - transform.position).normalized;
        FireProjectile(direction);
    }
}