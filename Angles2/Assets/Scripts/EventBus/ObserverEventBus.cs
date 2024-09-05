using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public ChangeRatioCommand(Action<float> ChangeRatioAction)
    {
        this.ChangeRatioAction = ChangeRatioAction;
    }

    Action<float> ChangeRatioAction;

    public override void Execute(float ratio)
    {
        ChangeRatioAction?.Invoke(ratio);
    }
}

public static class ObserverEventBus
{
    public enum State
    {
        OnDashRatioChange,
        OnChargeRatioChange,
        OnAddSkill,
        OnRemoveSkill,
        OnTurnOnOffDirection,
    }

    private static Dictionary<State, BaseCommand> commands = new Dictionary<State, BaseCommand>();

    public static void Register(State state, BaseCommand command)
    {
        commands.Add(state, command);
    }

    //이벤트 해제
    public static void Unregister(State state, BaseCommand command)
    {
        if (!commands.ContainsKey(state)) return;
        commands.Remove(state);
    }

    //이벤트 실행
    public static void Publish(State state)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute();
    }

    public static void Publish(State state, bool item)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(item);
    }

    public static void Publish(State state, float item)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(item);
    }

    public static void Publish(State state, BaseSkill.Name name, BaseSkill skill)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(name, skill);
    }

    public static void Publish(State state, BaseSkill.Name name)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(name);
    }

    public static void Clear()
    {
        commands.Clear();
    }
}
