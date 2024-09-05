using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnValueCommand : BaseCommand
{
    public ReturnValueCommand(Func<int> ReturnValueAction)
    {
        this.ReturnValueAction = ReturnValueAction;
    }

    Func<int> ReturnValueAction;

    public override void Execute(out int value)
    {
        if (ReturnValueAction == null) value = -1;
        value = ReturnValueAction();
    }
}

public class ChangeValueCommand : BaseCommand
{
    public ChangeValueCommand(Action<int> ChangeValueAction)
    {
        this.ChangeValueAction = ChangeValueAction;
    }

    Action<int> ChangeValueAction;

    public override void Execute(int cardCount)
    {
        ChangeValueAction?.Invoke(cardCount);
    }
}

public static class GameStateEventBus
{
    public enum State
    {
        ChangeCoin,
        ReturnCoin,

        ChangeEnemyDieCount,
        ReturnEnemyDieCount,
    }

    private static Dictionary<State, BaseCommand> commands = new Dictionary<State, BaseCommand>();

    public static void Register(State state, BaseCommand command)
    {
        if (!commands.ContainsKey(state))
            commands.Add(state, command);
    }

    //이벤트 해제
    public static void Unregister(State state, BaseCommand command)
    {
        if (!commands.ContainsKey(state)) return;
        commands.Remove(state);
    }

    //이벤트 실행
    public static void Publish(State state, out int value)
    {
        if (commands.ContainsKey(state))
        {
            commands[state].Execute(out value);
        }
        value = default;
    }

    public static void Publish(State state, int item)
    {
        if (!commands.ContainsKey(state)) return;
        commands[state].Execute(item);
    }

    public static void Clear()
    {
        commands.Clear();
    }
}