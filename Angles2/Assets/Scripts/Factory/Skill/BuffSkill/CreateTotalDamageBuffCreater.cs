using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTotalDamageBuffData : SkillData
{
    public CreateTotalDamageBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateTotalDamageBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateTotalDamageBuffCreater(SkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateTotalDamageBuffData data = _skillData as CreateTotalDamageBuffData;
        return new CreateTotalDamageBuff(data, _buffFactory);
    }
}
