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

public class MainEventBus : BaseEventBus<MainEventBus.State>
{
    public enum State
    {
        GameClear,
        GameOver,
        Pause,
    }
}