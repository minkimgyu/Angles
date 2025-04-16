using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Skill;
using Skill.Strategy;

public class Statikk : BaseSkill
{
    BaseFactory _effectFactory;
    StatikkData _data;

    public Statikk(StatikkData data, IUpgradeVisitor upgrader, BaseFactory effectFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader; // 이건 생성자에서 받아서 쓰기
        _effectFactory = effectFactory; // 이건 생성자에서 받아서 쓰기
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
        _targetingStrategy = new Skill.Strategy.CircleRangeTargetingStrategy(_caster, _data.Range, _data.TargetTypes);
        _actionStrategy = new Skill.Strategy.HitTargetStrategy(
            _caster,
            _upgradeableRatio,
            _data.AdRatio,
            _data.GroggyDuration
        );

        Color startColor = new Color(93f / 255f, 177f / 255f, 255f / 255f);
        Color endColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        _effectStrategy = new LaserEffectStrategy(BaseEffect.Name.KnockbackEffect, startColor, endColor, _effectFactory);
        _soundStrategy = new PlaySoundStrategy(ISoundPlayable.SoundName.Statikk);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        List<Vector2> targetPoints = new List<Vector2>();
        List<IDamageable> damageables = _targetingStrategy.GetDamageables(targetObject, new Skill.Strategy.CircleRangeTargetingStrategy.ChangeableData(_data.RangeMultiplier), _data.MaxTargetCount, out targetPoints);
        if (damageables == null || damageables.Count == 0) return false; // 타겟이 없는 경우

        _actionStrategy.Execute(damageables, new Skill.Strategy.HitTargetStrategy.ChangeableData(_data.Damage));

        Vector2 pos = _caster.GetComponent<Transform>().position;
        _effectStrategy.SpawnEffect(pos, targetPoints);
        _soundStrategy.PlaySound();
        return true;
    }
}
