using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour, IInputable
{
    [SerializeField] HandlerDictionary _handler;

    public void AddEvent(IInputable.Side side, IInputable.Type type, Action Event)
    {
        _handler[side].AddEvent(type, Event);
    }

    public void AddEvent(IInputable.Side side, IInputable.Type type, Action<Vector2> Event)
    {
        _handler[side].AddEvent(type, Event);
    }

    public void ClearEvent(IInputable.Side side)
    {
        _handler[side].ClearEvent();
    }

    public void RemoveEvent(IInputable.Side side, IInputable.Type type, Action Event)
    {
        _handler[side].RemoveEvent(type, Event);
    }

    public void RemoveEvent(IInputable.Side side, IInputable.Type type, Action<Vector2> Event)
    {
        _handler[side].RemoveEvent(type, Event);
    }
}
