using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpreadBullets : BaseSkill
{
    BulletData _bulletData;

    float _delay;
    float _force;
    float _distanceFromCaster;

    int _bulletCount;

    List<ITarget.Type> _targetTypes;
    List<ITarget> _targets;

    Timer _delayTimer;

    BaseFactory _weaponFactory;


    public SpreadBullets(SpreadBulletsData data, BaseFactory weaponFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _bulletData = data._bulletData;

        _delay = data._delay;
        _force = data._force;
        _distanceFromCaster = data._distanceFromCaster;

        _bulletCount = data._bulletCount;
        _targetTypes = data._targetTypes;

        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _weaponFactory = weaponFactory;
    }

    void ShootBullet(float angle)
    {
        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = _castingData.MyTransform.position + direction * _distanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Bullet);
        if (weapon == null) return;

        weapon.ResetData(_bulletData);

        weapon.Upgrade(UpgradePoint);
        weapon.ResetTargetTypes(_targetTypes);
        weapon.ResetPosition(spawnPosition, direction);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _force);
    }

    public override void OnUpdate()
    {

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;

                _delayTimer.Start(_delay);
                break;
            case Timer.State.Finish:
                Debug.Log("Shockwave");

                for (int i = 1; i <= _bulletCount; i++)
                {
                    float angle = 360f / _bulletCount * i; 
                    ShootBullet(angle);
                }
                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_targetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
