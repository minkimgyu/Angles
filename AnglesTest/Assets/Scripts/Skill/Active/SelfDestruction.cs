using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;

public class SelfDestruction : BaseSkill
{
    SelfDestructionData _data;
    BaseFactory _effectFactory;

    public SelfDestruction(SelfDestructionData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
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

        _delayStrategy = new DelayOnDamageStrategy(_data.Delay, _data.HpRatioOnInvoke, 
        () =>
        {
            CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
            if (castingComponent == null) return;
            castingComponent.CastSkill(_data.Delay);
        },
        () =>
        {
            List<IDamageable> damageables = _targetingStrategy.GetDamageables(new Skill.Strategy.CircleRangeTargetingStrategy.ChangeableData(_data.RangeMultiplier));
            _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

            Vector2 pos = _caster.GetComponent<Transform>().position;
            _effectStrategy.SpawnEffect(pos);
            _soundStrategy.PlaySound();
        });

        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio);

        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.Impact);
        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.ImpactEffect, _effectFactory);
    }
}