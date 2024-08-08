using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISkillUser
{
    void AddSkill(BaseSkill.Name name);
    void AddSkill(BaseSkill.Name skillName, BaseSkill skill) { }
}
