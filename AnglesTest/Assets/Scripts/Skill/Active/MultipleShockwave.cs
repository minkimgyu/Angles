using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class MultipleShockwave : BaseSkill
{
    MultipleShockwaveData _data;
    BaseFactory _effectFactory;

    public MultipleShockwave(MultipleShockwaveData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _targetingStrategy = new Skill.Strategy.CircleRangeTargetingStrategy(_caster, _data.Range, _data.TargetTypes);
        _detectingStrategy = new Skill.Strategy.TargetDetectingStrategy(_data.TargetTypes);

        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio
        );

        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.MultipleShockwaveEffect, _effectFactory);
        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.Impact);

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
               float sizeMultiplier = 1 + (_data.WaveSizeMultiply * waveCount);

               List<IDamageable> targets = _targetingStrategy.GetDamageables(new Skill.Strategy.CircleRangeTargetingStrategy.ChangeableData(sizeMultiplier));
               _actionStrategy.Execute(targets, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

               Vector2 pos = _caster.GetComponent<Transform>().position;
               _effectStrategy.SpawnEffect(pos, sizeMultiplier);
               _soundStrategy.PlaySound();
           }
       );
    }
}