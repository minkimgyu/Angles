using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

public interface ICaster
{
    void GetDamageData(DamageableData damageData) { }

    T GetComponent<T>();
    void AddSkill(BaseSkill.Name skillName, BaseSkill skill);
    List<SkillUpgradeData> ReturnSkillUpgradeDatas();
}