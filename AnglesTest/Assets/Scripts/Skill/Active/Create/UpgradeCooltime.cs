using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCooltime : BaseSkill
{
    UpgradeCooltimeData _data;

    public UpgradeCooltime(UpgradeCooltimeData data) : base(Type.Passive, data.MaxUpgradePoint)
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

        visitor.Upgrade(_data.CooltimeStatModifier, UpgradePoint - 1);
    }
}
