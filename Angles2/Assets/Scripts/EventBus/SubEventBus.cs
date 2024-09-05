using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemCommand : BaseCommand
{
    public DropItemCommand(Action<DropData, Vector3> DropItemAction)
    {
        this.DropItemAction = DropItemAction;
    }

    Action<DropData, Vector3> DropItemAction;

    public override void Execute(DropData value1, Vector3 value2)
    {
        DropItemAction?.Invoke(value1, value2);
    }
}

public class RegisterFollowerCommand : BaseCommand
{
    public RegisterFollowerCommand(Action<IFollowable> RegisterFollowerAction)
    {
        this.RegisterFollowerAction = RegisterFollowerAction;
    }

    Action<IFollowable> RegisterFollowerAction;

    public override void Execute(IFollowable followable)
    {
        RegisterFollowerAction?.Invoke(followable);
    }
}

public class RegisterSkillUpgradeableCommand : BaseCommand
{
    public RegisterSkillUpgradeableCommand(Action<ISkillUser> RegisterSkillUpgradeableAction)
    {
        this.RegisterSkillUpgradeableAction = RegisterSkillUpgradeableAction;
    }

    Action<ISkillUser> RegisterSkillUpgradeableAction;

    public override void Execute(ISkillUser cardCount)
    {
        RegisterSkillUpgradeableAction?.Invoke(cardCount);
    }
}

public class CreateCardCommand : BaseCommand
{
    public CreateCardCommand(Action<int> CreateCardAction)
    {
        this.CreateCardAction = CreateCardAction;
    }

    Action<int> CreateCardAction;

    public override void Execute(int cardCount)
    {
        CreateCardAction?.Invoke(cardCount);
    }
}

public class CreateReusableCardCommand : BaseCommand
{
    public CreateReusableCardCommand(Action<int, int> CreateReusableCardAction)
    {
        this.CreateReusableCardAction = CreateReusableCardAction;
    }

    Action<int, int> CreateReusableCardAction;

    public override void Execute(int cardCount, int recreateCount)
    {
        CreateReusableCardAction?.Invoke(cardCount, recreateCount);
    }
}

public static class SubEventBus
{
    public enum State
    {
        CreateCard,
        CreateReusableCard,

        DropItem,

        PlayerDie,

        RegisterSkillUpgradeable,
        RegisterFollableCamera,
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

    public static void Publish(State state, ISkillUser value1)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1);
    }

    public static void Publish(State state, IFollowable value1)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1);
    }

    public static void Publish(State state, DropData value1, Vector3 value2)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1, value2);
    }

    public static void Publish(State state, int value1)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1);
    }

    public static void Publish(State state, int value1, int value2)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(value1, value2);
    }

    public static void Clear()
    {
        commands.Clear();
    }
}