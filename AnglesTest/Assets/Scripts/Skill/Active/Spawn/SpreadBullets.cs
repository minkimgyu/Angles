using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;
using Skill.Strategy;

public class SpreadBullets : BaseSkill
{
    List<ITarget> _targets;

    Timer _delayTimer;
    BaseFactory _weaponFactory;
    SpreadBulletsData _data;

    public SpreadBullets(SpreadBulletsData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Basic, data.MaxUpgradePoint)
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
        _delayStrategy.ChangeDelay(_data.Delay);
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

        _actionStrategy = new SpreadBulletStrategy(
            _caster,
           _upgradeableRatio,
           _data.BulletCount,
           _data.AdRatio,
           _data.GroggyDuration,
           _data.DistanceFromCaster,
           _data.BulletName,
           _weaponFactory,
           _data.TargetTypes);

        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.SpreadBullets);

        _delayStrategy = new DelayRoutineStrategy(
           _data.Delay,
           () => _detectingStrategy.DetectTargets().Count > 0,
           () =>
           {
               CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
               if (castingComponent == null) return;

               castingComponent.CastSkill(_data.Delay);
           },
           () =>
           {
               _actionStrategy.Execute(new SpreadBulletStrategy.ChangeableData(_data.Damage, _data.Force));
               _soundStrategy.PlaySound();
           }
       );
    }
}
