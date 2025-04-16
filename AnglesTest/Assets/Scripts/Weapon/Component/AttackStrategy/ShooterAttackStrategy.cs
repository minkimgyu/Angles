using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class ShooterAttackStrategy : IWeaponActionStrategy
{
    protected ShooterData _shooterData;
    Transform _myTransform;
    float _waitFire;

    Func<List<ITarget>> GetTargets;

    protected ShooterAttackStrategy(ShooterData shooterData, Transform myTransform, Func<List<ITarget>> GetTargets)
    {
        _shooterData = shooterData;
        _myTransform = myTransform;
        this.GetTargets = GetTargets;
        _waitFire = 0;
    }

    public void OnUpdate()
    {
        ITarget target = ReturnCapturedTarget();
        if (target == null) return;

        Vector3 targetPos = target.GetPosition();
        Vector2 direction = (targetPos - _myTransform.position).normalized;
        FireProjectile(direction);
    }

    protected abstract BaseWeapon CreateProjectileWeapon();

    void FireProjectile(Vector2 direction)
    {
        _waitFire += Time.deltaTime;
        if (_shooterData.FireDelay > _waitFire) return;

        _waitFire = 0;

        BaseWeapon weapon = CreateProjectileWeapon();
        weapon.ResetPosition(_myTransform.position);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _shooterData.ShootForce);
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.ShooterFire, _myTransform.position);
    }

    ITarget ReturnCapturedTarget()
    {
        ITarget capturedTarget = null;

        List<ITarget> targets = GetTargets();

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if ((targets[i] as UnityEngine.Object) == null) continue;

            bool isTarget = targets[i].IsTarget(_shooterData.TargetTypes);
            if (isTarget == false) continue;

            capturedTarget = targets[i];
            break;
        }

        return capturedTarget;
    }
}
