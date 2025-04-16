using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Random = UnityEngine.Random;
using Skill;
using Skill.Strategy;

public class ShootMultipleLaser : BaseSkill
{
    List<ITarget> _targets;

    Timer _delayTimer;
    ShootMultipleLaserData _data;
    BaseFactory _effectFactory;

    public ShootMultipleLaser(ShootMultipleLaserData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();
        _effectFactory = effectFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio
        );

        Color startColor = new Color(183f / 255f, 47f / 255f, 253f / 255f);
        Color endColor = new Color(255f / 255f, 93f / 255f, 158f / 255f);
        _effectStrategy = new LaserEffectStrategy(BaseEffect.Name.LaserEffect, startColor, endColor, _effectFactory);
        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.SpreadBullets);

        Transform casterTransform = _caster.GetComponent<Transform>();
        _targetingStrategy = new RandomDirectionTargetingStrategy(_data.DistanceFromCaster, _data.MaxDistance, _data.ShootableLaserCount, _data.TotalLaserCount, casterTransform, _data.TargetTypes);

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
               List<Vector2> startPoints, hitPoints;
               List<IDamageable> targetDatas = _targetingStrategy.GetDamageables(out startPoints, out hitPoints);
               _actionStrategy.Execute(targetDatas, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

               _effectStrategy.SpawnEffect(startPoints, hitPoints);
               _soundStrategy.PlaySound();
           }
       );
    }
}
