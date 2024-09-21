using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCooltime : BaseSkill
{
    UpgradeCooltimeData _data;

    public UpgradeCooltime(UpgradeCooltimeData data) : base(Type.Active, data._maxUpgradePoint)
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
        _useConstraint = new NoConstraintComponent();
        UpgradeStat();
    }

    void UpgradeStat()
    {
        GameObject myObject = _castingData.MyObject;
        IStatUpgradable visitor = myObject.GetComponent<IStatUpgradable>();
        if (visitor == null) return;

        visitor.Upgrade(_data._cooltimeDatas[UpgradePoint]);
    }
}
