using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;

public class Knockback : BaseSkill
{
    BaseFactory _effectFactory;
    KnockbackData _data;

    public Knockback(KnockbackData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
        _upgrader = upgrader;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);

        _useConstraintStrategy = new CooltimeConstraintStrategy(_data, _upgradeableRatio);
        _targetingStrategy = new BoxRangeTargetingStrategy(_caster, _data.Size.V2, _data.Offset.V2, _data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio,
           _data.GroggyDuration
        );

        _effectStrategy = new DirectionParticleEffectStrategy(BaseEffect.Name.KnockbackEffect, _effectFactory);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        List<IDamageable> damageables = _targetingStrategy.GetDamageables(targetObject, new Skill.Strategy.BoxRangeTargetingStrategy.ChangeableData(_data.RangeMultiplier));
        if (damageables == null || damageables.Count == 0) return false; // 타겟이 없는 경우

        _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

        Transform casterTransform = _caster.GetComponent<Transform>();
        _effectStrategy.SpawnEffect(casterTransform.position, casterTransform.right);
        return true;
    }
}
