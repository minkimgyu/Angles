using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class UpgradeShooting : BaseSkill
{
    UpgradeShootingData _data;

    public UpgradeShooting(UpgradeShootingData data) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
    }

    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _actionStrategy = new UpgradeStatStrategy(_caster, _data.ShootingStatModifier, () => { return UpgradePoint; });
    }
}
