using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GameEndCommand : BaseCommand
{
    public GameEndCommand(Action GameEndAction)
    {
        this.GameEndAction = GameEndAction;
    }

    Action GameEndAction;

    public override void Execute() 
    {
        GameEndAction?.Invoke();
    }
}

public static class MainEventBus
{
    public enum State
    {
        GameClear,
        GameOver,
        Pause,
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

    public static void Clear()
    {
        commands.Clear();
    }
}