using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _useConstraintStrategy = new NoConstraintStrategy();
        _targetStrategy = new NoTargetingStrategy();
        _delayStrategy = new NoDelayStrategy();
        _actionStrategy = new ReviveImpactStrategy(_caster, _upgradeableRatio, _data.Range, _data.TargetTypes, _effectFactory);
    }
}
