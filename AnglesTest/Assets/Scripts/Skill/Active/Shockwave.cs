using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;

public class Shockwave : BaseSkill
{
    ShockwaveData _data;
    BaseFactory _effectFactory;
    Timer _delayTimer;
    List<ITarget> _targets;

    public Shockwave(ShockwaveData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();

        _upgrader = upgrader;
        _effectFactory = effectFactory;
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

        _targetingStrategy = new Skill.Strategy.CircleRangeTargetingStrategy(_caster, _data.Range, _data.TargetTypes);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

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
                List<IDamageable> damageables = _targetingStrategy.GetDamageables();
                _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

                Vector2 pos = _caster.GetComponent<Transform>().position;
                _effectStrategy.SpawnEffect(pos);
                _soundStrategy.PlaySound();
            }
        );

        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.Knockback);
        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.ShockwaveEffect, _effectFactory);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio
        );
    }
}
