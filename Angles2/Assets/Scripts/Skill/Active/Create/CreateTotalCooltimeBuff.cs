using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTotalCooltimeBuff : BaseSkill
{
    BaseFactory _buffFactory;

    public CreateTotalCooltimeBuff(CreateTotalCooltimeBuffData data, BaseFactory buffFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _buffFactory = buffFactory;
    }

    void AddBuff()
    {
        GameObject myObject = _castingData.MyObject;
        IBuffUsable buffUsable = myObject.GetComponent<IBuffUsable>();
        if (buffUsable == null) return;

        BaseBuff totalDamageBuff = _buffFactory.Create(BaseBuff.Name.TotalCooltime);
        buffUsable.AddBuff(BaseBuff.Name.TotalDamage, totalDamageBuff);
    }

    protected override void OnUpgradeRequested()
    {
        AddBuff();
    }

    public override void OnAdd()
    {
        AddBuff();
    }
}
