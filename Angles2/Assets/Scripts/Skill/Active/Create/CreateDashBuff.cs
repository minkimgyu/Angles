using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDashBuff : BaseSkill
{
    BaseFactory _buffFactory;

    public CreateDashBuff(CreateDashBuffData data, BaseFactory buffFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _buffFactory = buffFactory;
    }

    void AddBuff()
    {
        GameObject myObject = _castingData.MyObject;
        IBuffUsable buffUsable = myObject.GetComponent<IBuffUsable>();
        if (buffUsable == null) return;

        BaseBuff totalDamageBuff = _buffFactory.Create(BaseBuff.Name.Dash);
        buffUsable.AddBuff(BaseBuff.Name.TotalDamage, totalDamageBuff);
    }

    public override void OnAdd()
    {
        AddBuff();
    }
}
