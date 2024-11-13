using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCooltime : BaseSkill
{
    UpgradeCooltimeData _data;

    public UpgradeCooltime(UpgradeCooltimeData data) : base(Type.Passive, data._maxUpgradePoint)
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
        GameObject myObject = _caster.GetComponent<GameObject>();
        IStatUpgradable visitor = myObject.GetComponent<IStatUpgradable>();
        if (visitor == null) return;

        visitor.Upgrade(_data._cooltimeStatModifier, UpgradePoint - 1);
    }
}
