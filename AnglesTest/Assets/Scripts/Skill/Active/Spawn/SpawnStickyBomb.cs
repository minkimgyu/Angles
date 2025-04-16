using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;
using Skill.Strategy;

public class SpawnStickyBomb : BaseSkill
{
    BaseFactory _weaponFactory;
    SpawnStickyBombData _data;

    public SpawnStickyBomb(SpawnStickyBombData data, IUpgradeVisitor upgrader, BaseFactory weaponFactory) : base(Type.Active, data.MaxUpgradePoint)
    {
        _upgrader = upgrader;
        _data = data;
        _weaponFactory = weaponFactory;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _useConstraintStrategy = new CooltimeConstraintStrategy(_data, _upgradeableRatio);
        _targetingStrategy = new Skill.Strategy.ContactTargetingStrategy(_data.TargetTypes);

        _actionStrategy = new SpawnStickyBombStrategy(_caster,
            _upgradeableRatio,
            _data.AdRatio,
            _data.Delay,
            _data.GroggyDuration,
            _data.TargetTypes,
            _weaponFactory);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        _upgrader.Visit(this, _data);
    }

    public override bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
    {
        IFollowable followable = _targetingStrategy.GetFollowableTarget(targetObject);
        if (followable == null) return false;

        _actionStrategy.Execute(followable, new Skill.Strategy.SpawnStickyBombStrategy.ChangeableData(_data.Damage));
        return true;
    }
}
