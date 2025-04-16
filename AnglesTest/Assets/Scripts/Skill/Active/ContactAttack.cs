using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;

public class ContactAttack : BaseSkill
{
    ContactAttackData _data;
    BaseFactory _effectFactory;

    public ContactAttack(ContactAttackData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _effectFactory = effectFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _targetingStrategy = new Skill.Strategy.ContactTargetingStrategy(_data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio,
           _data.GroggyDuration);

        _effectStrategy = new ParticleEffectStrategy(BaseEffect.Name.HitEffect, _effectFactory);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        IDamageable damageable = _targetingStrategy.GetDamageable(targetObject);
        if (damageable == null) return false; // 타겟이 없는 경우

        _actionStrategy.Execute(damageable, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));
        _effectStrategy.SpawnEffect(contactPos);
        return true;
    }
}
