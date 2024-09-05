using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDashBuffData : BaseSkillData
{
    public CreateDashBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateDashBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateDashBuffCreater(BaseSkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateDashBuffData data = _buffData as CreateDashBuffData;
        return new CreateDashBuff(data, _buffFactory);
    }
}