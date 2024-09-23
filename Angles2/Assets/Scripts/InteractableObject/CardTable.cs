using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardTable : MonoBehaviour, IInteractable
{
    OutlineComponent _outlineComponent;
    bool _isActive;
    int _cardCount;

    public void Initialize(CardTableData data)
    {
        _cardCount = data._cardCount;

        _isActive = true;
        _outlineComponent = GetComponentInChildren<OutlineComponent>();
        _outlineComponent.Initialize();
    }

    public void OnInteractEnter(IInteracter interacter)
    {
        if (_isActive == false) return;

        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnInteract);
    }

    public void OnInteract(IInteracter interacter)
    {
        if (_isActive == false) return;

        _isActive = false;
        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnDisabled);
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateCard, interacter.ReturnSkillUser(), _cardCount);
    }

    public void OnInteractExit(IInteracter interacter)
    {
        if (_isActive == false) return;

        _outlineComponent.OnOutlineChange(OutlineComponent.Condition.OnIdle);
    }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}