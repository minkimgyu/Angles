using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISkillAddable
{
    void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { }
}

public interface ISkillUpgradeable
{
    List<SkillUpgradeData> ReturnSkillUpgradeDatas();
}

public interface ISkillUser : ISkillAddable, ISkillUpgradeable
{
}