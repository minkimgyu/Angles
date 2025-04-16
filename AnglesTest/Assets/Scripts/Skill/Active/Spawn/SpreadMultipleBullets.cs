using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

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

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

        _actionStrategy = new SpreadBulletStrategy(
            _caster,
           _upgradeableRatio,
           _data.BulletCount,
           _data.AdRatio,
           0,
           _data.DistanceFromCaster,
           _data.BulletName,
           _weaponFactory,
           _data.TargetTypes);

        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.SpreadBullets);

        _delayStrategy = new WaveDelayRoutineStrategy(
           _data.Delay,
           _data.MaxWaveCount,
           _data.WaveDelay,
           () => _detectingStrategy.DetectTargets().Count > 0,
           () =>
           {
               CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
               if (castingComponent == null) return;

               castingComponent.CastSkill(_data.Delay);
           },
           (waveCount) =>
           {
               _actionStrategy.Execute(new SpreadBulletStrategy.ChangeableData(_data.Damage, _data.Force));
               _soundStrategy.PlaySound();
           }
       );
    }
}
