using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseInputHandler : MonoBehaviour
{
    protected Action OnInputStartRequested;
    protected Action<Vector2> OnInputRequested;
    protected Action OnInputEndRequested;

    protected Action OnDoubleTabRequested;

    public void AddEvent(IInputable.Type type, Action InputEvent)
    {
        switch (type)
        {
            case IInputable.Type.OnInputStart:
                OnInputStartRequested += InputEvent;
                break;
            case IInputable.Type.OnInputEnd:
                OnInputEndRequested += InputEvent;
                break;
            case IInputable.Type.OnDoubleTab:
                OnDoubleTabRequested += InputEvent;
                break;
            default:
                break;
        }
    }

    public void AddEvent(IInputable.Type type, Action<Vector2> InputEvent)
    {
        switch (type)
        {
            case IInputable.Type.OnInput:
                OnInputRequested += InputEvent;
                break;
            default:
                break;
        }
    }

    public void ClearEvent()
    {
        OnInputStartRequested = null;
        OnInputRequested = null;
        OnInputEndRequested = null;
        OnDoubleTabRequested = null;
    }

    public void RemoveEvent(IInputable.Type type, Action InputEvent)
    {
        switch (type)
        {
            case IInputable.Type.OnInputStart:
                OnInputStartRequested -= InputEvent;
                break;
            case IInputable.Type.OnInputEnd:
                OnInputEndRequested -= InputEvent;
                break;
            case IInputable.Type.OnDoubleTab:
                OnDoubleTabRequested -= InputEvent;
                break;
            default:
                break;
        }
    }

    public void RemoveEvent(IInputable.Type type, Action<Vector2> InputEvent)
    {
        switch (type)
        {
            case IInputable.Type.OnInput:
                OnInputRequested -= InputEvent;
                break;
            default:
                break;
        }
    }
}
