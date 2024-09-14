using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDashBuffData : SkillData
{
    public CreateDashBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateDashBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateDashBuffCreater(SkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateDashBuffData data = _skillData as CreateDashBuffData;
        return new CreateDashBuff(data, _buffFactory);
    }
}