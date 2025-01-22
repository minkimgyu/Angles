using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : MonoBehaviour, IInteractable
{
    Vector2 _movePos;
    bool _isActive;

    Action OnInteractRequested;

    public void Initialize(Action OnInteractRequested)
    {
        this.OnInteractRequested = OnInteractRequested;
        Disable();
    }

    public void Active(Vector2 movePos)
    {
        _movePos = movePos;
        _isActive = true;
    }

    public void Disable()
    {
        _isActive = false;
    }

    public void OnInteractEnter(IInteracter interacter) { }

    public void OnInteract(IInteracter interacter)
    {
        if (_isActive == false) return;
        OnInteractRequested?.Invoke();

        interacter.MovePosition(_movePos);
    }

    public void OnInteractExit(IInteracter interacter) { }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
