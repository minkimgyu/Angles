using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class UpgradeDamage : BaseSkill
{
    UpgradeDamageData _data;

    public UpgradeDamage(UpgradeDamageData data) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _actionStrategy = new UpgradeStatStrategy(_caster, _data.DamageStatModifier, () => { return UpgradePoint; });
    }
}
