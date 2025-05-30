using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill;

abstract public class BaseCommand
{
    public virtual void Execute(out int value) { value = default; }

   
    public virtual void Execute() { }
    public virtual void Execute(float value) { }
    public virtual void Execute(bool value) { }
    public virtual void Execute(IFollowable followable) { }

    public virtual void Execute(float fillRatio, float alphaChangeRatio) { }

    public virtual void Execute(BaseSkill.Name value) { }
    public virtual void Execute(BaseSkill.Name value1, BaseSkill value2) { }
    public virtual void Execute(DropData value1, Vector3 value2) { }

    public virtual void Execute(ICaster caster, int value) { }
    public virtual void Execute(ICaster caster, int cardCount, int recreateCount) { }
}