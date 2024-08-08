using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInputable
{
    public enum Side
    {
        Left,
        Right
    }

    public enum Type
    {
        None,
        OnInputStart,
        OnInput,
        OnInputEnd,
        OnDoubleTab
    }

    void AddEvent(Side side, Type type, Action Event);

    void AddEvent(Side side, Type type, Action<Vector2> Event);

    void ClearEvent(Side side);

    void RemoveEvent(Side side, Type type, Action Event);

    void RemoveEvent(Side side, Type type, Action<Vector2> Event);
}
