using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    OutlineComponent _outlineComponent;
    Vector2 _movePos;
    bool _isActive;

    System.Action OnInteractRequested;

    public void Initialize(System.Action OnInteractRequested)
    {
        this.OnInteractRequested = OnInteractRequested;
        _outlineComponent = GetComponentInChildren<OutlineComponent>();
        _outlineComponent.Initialize();

        Disable();
    }

    public void Active(Vector2 movePos)
    {
        _movePos = movePos;
        _isActive = true;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnEnabled);
    }

    public void Disable()
    {
        _isActive = false;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDisabled);
    }

    public void OnInteractEnter(IInteracter interacter) { }

    public void OnInteract(IInteracter interacter)
    {
        if (_isActive == false) return;
        OnInteractRequested?.Invoke();

        interacter.MovePosition(_movePos);
    }

    public void OnInteractExit(IInteracter interacter) { }

    public Object ReturnObject()
    {
        return this;
    }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
