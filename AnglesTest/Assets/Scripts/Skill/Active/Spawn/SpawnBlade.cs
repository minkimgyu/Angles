using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;
using Skill.Strategy;

public class SpawnBlade : BaseSkill
{
    SpawnBladeData _data;
    BaseFactory _weaponFactory;

    public SpawnBlade(SpawnBladeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) :base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
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
        _targetingStrategy = new Skill.Strategy.ContactTargetingStrategy(_data.TargetTypes);

        _actionStrategy = new SpawnBladeStrategy(
            _caster,
           _upgradeableRatio,
           _data.AdRatio,
           _data.Force,
           //_data.Damage,
           //_data.SizeMultiplier,
           //_data.Lifetime,
           _data.GroggyDuration,
           _data.TargetTypes,
           _weaponFactory);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        ITarget target = _targetingStrategy.GetTarget(targetObject);
        if (target == null) return false;

        _actionStrategy.Execute(new SpawnBladeStrategy.ChangeableData(_data.Damage, _data.SizeMultiplier, _data.Lifetime));
        return true;
    }
}
