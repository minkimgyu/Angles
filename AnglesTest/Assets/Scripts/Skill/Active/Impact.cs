using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;
public class Impact : BaseSkill
{
    ImpactData _data;
    BaseFactory _effectFactory;

    public Impact(ImpactData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _effectFactory = effectFactory;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);

        _useConstraintStrategy = new RandomConstraintStrategy(_data, _upgradeableRatio);
        _targetingStrategy = new Skill.Strategy.CircleRangeTargetingStrategy(_caster, _data.Range, _data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio,
           _data.GroggyDuration);
        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.ImpactEffect, _effectFactory);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        List<IDamageable> damageables = _targetingStrategy.GetDamageables(targetObject, new Skill.Strategy.CircleRangeTargetingStrategy.ChangeableData(_data.RangeMultiplier));
        if (damageables == null || damageables.Count == 0) return false; // 타겟이 없는 경우

        _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

        Vector2 pos = _caster.GetComponent<Transform>().position;
        _effectStrategy.SpawnEffect(pos);
        return true;
    }
}
