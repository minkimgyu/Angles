using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

public class AddSkillCommand : BaseCommand
{
    public AddSkillCommand(Action<BaseSkill.Name, BaseSkill> AddSkillAction)
    {
        this.AddSkillAction = AddSkillAction;
    }

    Action<BaseSkill.Name, BaseSkill> AddSkillAction;

    public override void Execute(BaseSkill.Name name, BaseSkill skill)
    {
        AddSkillAction?.Invoke(name, skill);
    }
}

public class RemoveSkillCommand : BaseCommand
{
    public RemoveSkillCommand(Action<BaseSkill.Name, BaseSkill> RemoveSkillAction)
    {
        this.RemoveSkillAction = RemoveSkillAction;
    }

    Action<BaseSkill.Name, BaseSkill> RemoveSkillAction;

    public override void Execute(BaseSkill.Name name, BaseSkill skill)
    {
        RemoveSkillAction?.Invoke(name, skill);
    }
}

public class TurnOnOffCommand : BaseCommand
{
    public TurnOnOffCommand(Action<bool> TurnOnOffDirectionAction)
    {
        this.TurnOnOffDirectionAction = TurnOnOffDirectionAction;
    }

    Action<bool> TurnOnOffDirectionAction;

    public override void Execute(bool turnOnOff)
    {
        TurnOnOffDirectionAction?.Invoke(turnOnOff);
    }
}

public class ChangeRatioCommand : BaseCommand
{
    public ChangeRatioCommand(Action<float, float> ChangeRatioAction)
    {
        this.ChangeRatioAction = ChangeRatioAction;
    }

    Action<float, float> ChangeRatioAction;

    public override void Execute(float fillRatio, float alphaChangeRatio)
    {
        ChangeRatioAction?.Invoke(fillRatio, alphaChangeRatio);
    }
}

public class ObserverEventBus : BaseEventBus<ObserverEventBus.State>
{
    public enum State
    {
        OnDashRatioChange,
        OnChargeRatioChange,
        OnAddSkill,
        OnRemoveSkill,
        OnTurnOnOffDirection,
    }
}
