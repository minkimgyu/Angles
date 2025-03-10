using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDamage : BaseSkill
{
    UpgradeDamageData _data;

    public UpgradeDamage(UpgradeDamageData data) : base(Type.Passive, data.MaxUpgradePoint)
    {
        _data = data;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        UpgradeStat();
    }

    public override void OnAdd()
    {
        UpgradeStat();
    }

    void UpgradeStat()
    {
        IStatUpgradable visitor = _caster.GetComponent<IStatUpgradable>();
        if (visitor == null) return;

        visitor.Upgrade(_data.DamageStatModifier, UpgradePoint - 1);
    }
}
