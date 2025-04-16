using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
using Skill.Strategy;

public class UpgradeCooltime : BaseSkill
{
    UpgradeCooltimeData _data;

    public UpgradeCooltime(UpgradeCooltimeData data) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
    }


    public override void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
    {
        base.Initialize(upgradeableRatio, caster);
        _actionStrategy = new UpgradeStatStrategy(_caster, _data.CooltimeStatModifier, () => { return UpgradePoint; });
    }
}
