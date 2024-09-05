using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTotalCooltimeBuffData : BaseSkillData
{
    public CreateTotalCooltimeBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateTotalCooltimeBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateTotalCooltimeBuffCreater(BaseSkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateTotalCooltimeBuffData data = _buffData as CreateTotalCooltimeBuffData;
        return new CreateTotalCooltimeBuff(data, _buffFactory);
    }
}