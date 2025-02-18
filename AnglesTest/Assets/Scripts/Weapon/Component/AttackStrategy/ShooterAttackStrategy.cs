using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ShooterAttackStrategy : IAttackStrategy
{
    List<ITarget> _targetDatas;
    protected ShooterData _shooterData;
    Transform _myTransform;
    float _waitFire;

    protected ShooterAttackStrategy(ShooterData shooterData, Transform myTransform)
    {
        _shooterData = shooterData;
        _myTransform = myTransform;
        _waitFire = 0;
        _targetDatas = new List<ITarget>();
    }

    public void OnTargetEnter(ITarget target) 
    {
        _targetDatas.Add(target);
    }

    public void OnTargetExit(ITarget target) 
    {
        _targetDatas.Remove(target);
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

        for (int i = _targetDatas.Count - 1; i >= 0; i--)
        {
            if ((_targetDatas[i] as UnityEngine.Object) == null) continue;

            bool isTarget = _targetDatas[i].IsTarget(_shooterData.DamageableData._targetType);
            if (isTarget == false) continue;

            capturedTarget = _targetDatas[i];
            break;
        }

        return capturedTarget;
    }
}
