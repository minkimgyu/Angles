using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShootingBuffData : SkillData
{
    public CreateShootingBuffData(int maxUpgradePoint) : base(maxUpgradePoint) { }
}

public class CreateShootingBuffCreater : SkillCreater
{
    BaseFactory _buffFactory;

    public CreateShootingBuffCreater(SkillData data, BaseFactory buffFactory) : base(data)
    {
        _buffFactory = buffFactory;
    }

    public override BaseSkill Create()
    {
        CreateShootingBuffData data = _skillData as CreateShootingBuffData;
        return new CreateShootingBuff(data, _buffFactory);
    }
}