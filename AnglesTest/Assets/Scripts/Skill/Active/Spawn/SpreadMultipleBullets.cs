using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadMultipleBullets : BaseSkill
{
    int _waveCount;
    Timer _waveTimer;

    List<ITarget> _targets;

    Timer _delayTimer;
    BaseFactory _weaponFactory;
    SpreadMultipleBulletsData _data;

    public SpreadMultipleBullets(SpreadMultipleBulletsData data, BaseFactory weaponFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _waveTimer = new Timer();
        _targets = new List<ITarget>();

        _weaponFactory = weaponFactory;
    }

    void ShootBullet(float angle)
    {
        Transform casterTransform = _caster.GetComponent<Transform>();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.SpreadBullets, casterTransform.position, 0.3f);

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = casterTransform.position + direction * _data.DistanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.PentagonicBullet);
        if (weapon == null) return;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        List<WeaponDataModifier> modifiers = new List<WeaponDataModifier>();
        modifiers.Add(new WeaponDamageModifier(damageData));

        weapon.ModifyData(modifiers);
        weapon.Activate();

        weapon.ResetPosition(spawnPosition, direction);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        projectile.Shoot(direction, _data.Force);
    }

    public override void OnUpdate()
    {

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;
                _delayTimer.Start(_data.Delay);
                break;
            case Timer.State.Finish:

                if (_waveTimer.CurrentState == Timer.State.Ready || _waveTimer.CurrentState == Timer.State.Finish)
                {
                    for (int i = 1; i <= _data.BulletCount; i++)
                    {
                        float angle = 360f / _data.BulletCount * i;
                        ShootBullet(angle);
                    }

                    _waveTimer.Reset();
                    _waveTimer.Start(_data.WaveDelay);
                    _waveCount++;

                    Debug.Log(_waveCount);
                    if (_waveCount == _data.MaxWaveCount)
                    {
                        _waveCount = 0;
                        _waveTimer.Reset();
                        _delayTimer.Reset();
                    }
                }
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
