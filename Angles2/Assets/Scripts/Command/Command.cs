using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Command<T1>
{
    Action<T1> _action;

    public Command(Action<T1> excuteEvent)
    {
        _action = excuteEvent;
    }

    public void Execute(T1 value)
    {
        _action?.Invoke(value);
    }
}

public class Command<T1,T2>
{
    public Command(Action<T1, T2> excuteEvent)
    {
        _excuteEvent = excuteEvent;
    }

    Action<T1, T2> _excuteEvent;

    public void Execute(T1 value1, T2 value2)
    {
        _excuteEvent?.Invoke(value1, value2);
    }
}

public class Command<T1, T2, T3>
{
    public Command(Action<T1, T2, T3> excuteEvent)
    {
        _excuteEvent = excuteEvent;
    }

    Action<T1, T2, T3> _excuteEvent;

    public void Execute(T1 value1, T2 value2, T3 value3)
    {
        _excuteEvent?.Invoke(value1, value2, value3);
    }
}