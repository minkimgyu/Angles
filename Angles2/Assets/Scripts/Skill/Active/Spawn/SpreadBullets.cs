using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpreadBullets : BaseSkill
{
    List<ITarget> _targets;

    Timer _delayTimer;
    BaseFactory _weaponFactory;
    SpreadBulletsData _data;

    public SpreadBullets(SpreadBulletsData data, BaseFactory weaponFactory) : base(Type.Basic, data._maxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _weaponFactory = weaponFactory;
    }

    public override void OnAdd()
    {
        _useConstraint = new NoConstraintComponent();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    void ShootBullet(float angle)
    {
        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = _castingData.MyTransform.position + direction * _data._distanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Bullet);
        if (weapon == null) return;

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(_data._damage));

        //weapon.ResetData(_data._bulletData);
        //weapon.ResetTargetTypes(_data._targetTypes);

        weapon.ModifyData(modifiers);
        weapon.ResetPosition(spawnPosition, direction);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _data._force);
    }

    public override void OnUpdate()
    {

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;

                CastingComponent castingComponent = _castingData.MyObject.GetComponent<CastingComponent>();
                if (castingComponent == null) break;

                castingComponent.CastSkill(_data._delay);
                _delayTimer.Start(_data._delay);
                break;
            case Timer.State.Finish:
                Debug.Log("Shockwave");

                for (int i = 1; i <= _data._bulletCount; i++)
                {
                    float angle = 360f / _data._bulletCount * i; 
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
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_data._targetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
