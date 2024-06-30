using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
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

    [SerializeField] HandlerDictionary _handler;

    static InputManager _instance;

    private void Awake()
    {
        if(_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    public static void AddEvent(Side side, Type type, Action Event)
    {
        _instance._handler[side].AddEvent(type, Event);
    }

    public static void AddEvent(Side side, Type type, Action<Vector2> Event)
    {
        _instance._handler[side].AddEvent(type, Event);
    }

    public static void ClearEvent(Side side)
    {
        _instance._handler[side].ClearEvent();
    }

    public static void RemoveEvent(Side side, Type type, Action Event)
    {
        _instance._handler[side].RemoveEvent(type, Event);
    }

    public static void RemoveEvent(Side side, Type type, Action<Vector2> Event)
    {
        _instance._handler[side].RemoveEvent(type, Event);
    }
}
