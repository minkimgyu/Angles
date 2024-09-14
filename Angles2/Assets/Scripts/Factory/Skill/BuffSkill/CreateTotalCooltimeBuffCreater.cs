using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTotalCooltimeBuffData : SkillData
{
    public CreateTotalCooltimeBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateTotalCooltimeBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateTotalCooltimeBuffCreater(SkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateTotalCooltimeBuffData data = _skillData as CreateTotalCooltimeBuffData;
        return new CreateTotalCooltimeBuff(data, _buffFactory);
    }
}