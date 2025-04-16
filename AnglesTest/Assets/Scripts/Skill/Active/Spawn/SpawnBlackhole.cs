using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class SpawnBlackhole : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnBlackholeData _data;

    public SpawnBlackhole(SpawnBlackholeData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _data = data;
        _upgrader = upgrader;
        _weaponFactory = weaponFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _useConstraintStrategy = new RandomConstraintStrategy(_data, _upgradeableRatio);
        _targetingStrategy = new Skill.Strategy.ContactTargetingStrategy(_data.TargetTypes);
        _actionStrategy = new SpawnBlackholeStrategy(
            _caster,
            _upgradeableRatio,
            //_data.Damage,
            //_data.,
            //_data.SizeMultiplier,
            _data.GroggyDuration,
            _weaponFactory,
            _data.TargetTypes);
    }


    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        ITarget target = _targetingStrategy.GetTarget(targetObject);
        if (target == null) return false;

        _actionStrategy.Execute(new Skill.Strategy.SpawnBlackholeStrategy.ChangeableData(_data.Damage, _data.Lifetime, _data.SizeMultiplier));
        return true;
    }
}
