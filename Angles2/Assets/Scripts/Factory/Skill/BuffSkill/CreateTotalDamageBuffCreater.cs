using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTotalDamageBuffData : BaseSkillData
{
    public CreateTotalDamageBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateTotalDamageBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateTotalDamageBuffCreater(BaseSkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateTotalDamageBuffData data = _buffData as CreateTotalDamageBuffData;
        return new CreateTotalDamageBuff(data, _buffFactory);
    }
}
