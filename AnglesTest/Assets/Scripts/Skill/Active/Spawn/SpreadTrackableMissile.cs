using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadTrackableMissile : BaseSkill
{
    List<ITarget> _targets;

    Timer _delayTimer;
    BaseFactory _weaponFactory;
    SpreadTrackableMissilesData _data;

    public SpreadTrackableMissile(SpreadTrackableMissilesData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    void ShootBullet(float angle)
    {
        Transform casterTransform = _caster.GetComponent<Transform>();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.SpreadBullets, casterTransform.position, 0.3f);

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = casterTransform.position + direction * _data.DistanceFromCaster;

        BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.TrackableMissile);
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

        TrackableMissileDataModifier bulletDataModifier = new TrackableMissileDataModifier(damageData);

        weapon.ModifyData(bulletDataModifier);
        weapon.Activate();

        weapon.ResetPosition(spawnPosition, direction);

        IProjectable projectile = weapon.GetComponent<IProjectable>();
        if (projectile == null) return;

        Player player = Object.FindObjectOfType<Player>();
        if(player == null) return;

        projectile.Shoot(player, _data.Force);
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

                for (int i = 1; i <= _data.BulletCount; i++)
                {
                    float angle = 360f / _data.BulletCount * i;
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
