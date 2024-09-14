using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShootingBuff : BaseSkill
{
    BaseFactory _buffFactory;

    public CreateShootingBuff(CreateShootingBuffData data, BaseFactory buffFactory) : base(Type.Active, data._maxUpgradePoint)
    {
        _buffFactory = buffFactory;
    }

    void AddBuff()
    {
        GameObject myObject = _castingData.MyObject;
        IBuffUsable buffUsable = myObject.GetComponent<IBuffUsable>();
        if (buffUsable == null) return;

        BaseBuff totalDamageBuff = _buffFactory.Create(BaseBuff.Name.Shooting);
        buffUsable.AddBuff(BaseBuff.Name.TotalDamage, totalDamageBuff);
    }

    public override void OnAdd()
    {
        AddBuff();
    }
}
