using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    OutlineComponent _outlineComponent;
    Vector2 _movePos;
    bool _isActive;

    public void Initialize()
    {
        _outlineComponent = GetComponentInChildren<OutlineComponent>();

        _isActive = false;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnEnabled);
    }

    public void Active(Vector2 movePos)
    {
        _movePos = movePos;
        _isActive = true;
    }

    public void OnInteractEnter(InteractEnterData data) { }

    public void OnInteract(InteractData data)
    {
        if (_isActive == true) return;
        data.ResetPosition?.Invoke(_movePos);
    }

    public void OnInteractExit(InteractExitData data) { }
}
