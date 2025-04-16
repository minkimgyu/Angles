using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class ReviveImpact : BaseSkill
{
    ReviveImpactData _data;
    BaseFactory _effectFactory;

    public ReviveImpact(ReviveImpactData data, BaseFactory effectFactory) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _targetingStrategy = new Skill.Strategy.CircleRangeTargetingStrategy(caster, _data.Range, _data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio
        );

        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.ImpactEffect, _effectFactory);
    }

    public override void OnRevive()
    {
        List<IDamageable> damageables = _targetingStrategy.GetDamageables();
        if (damageables.Count == 0) return; // 타겟이 없는 경우

        _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

        Vector2 pos = _caster.GetComponent<Transform>().position;
        _effectStrategy.SpawnEffect(pos);
        return;
    }
}
