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

    public void AddEvent(InputController.Type type, Action InputEvent)
    {
        switch (type)
        {
            case InputController.Type.OnInputStart:
                OnInputStartRequested += InputEvent;
                break;
            case InputController.Type.OnInputEnd:
                OnInputEndRequested += InputEvent;
                break;
            case InputController.Type.OnDoubleTab:
                OnDoubleTabRequested += InputEvent;
                break;
            default:
                break;
        }
    }

    public void AddEvent(InputController.Type type, Action<Vector2> InputEvent)
    {
        switch (type)
        {
            case InputController.Type.OnInput:
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

    public void RemoveEvent(InputController.Type type, Action InputEvent)
    {
        switch (type)
        {
            case InputController.Type.OnInputStart:
                OnInputStartRequested -= InputEvent;
                break;
            case InputController.Type.OnInputEnd:
                OnInputEndRequested -= InputEvent;
                break;
            case InputController.Type.OnDoubleTab:
                OnDoubleTabRequested -= InputEvent;
                break;
            default:
                break;
        }
    }

    public void RemoveEvent(InputController.Type type, Action<Vector2> InputEvent)
    {
        switch (type)
        {
            case InputController.Type.OnInput:
                OnInputRequested -= InputEvent;
                break;
            default:
                break;
        }
    }
}
