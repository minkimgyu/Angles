using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDamageData : SkillData
{
    public List<StatUpgrader.DamageData> _damageDatas;
    public UpgradeDamageData(int maxUpgradePoint, List<StatUpgrader.DamageData> damageDatas) : base(maxUpgradePoint) 
    {
        _damageDatas = damageDatas;
    }

    public override SkillData Copy()
    {
        return new UpgradeDamageData(_maxUpgradePoint, _damageDatas);
    }
}

public class UpgradeDamageCreater : SkillCreater
{
    public UpgradeDamageCreater(SkillData data) : base(data) { }

    public override BaseSkill Create()
    {
        UpgradeDamageData data = _skillData as UpgradeDamageData;
        return new UpgradeDamage(data);
    }
}
