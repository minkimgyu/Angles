using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour, IInteractable
{
    OutlineComponent _outlineComponent;
    bool _isActive;

    int _cardCount;
    int _recreateCount;

    public void Initialize(ShopData data)
    {
        _cardCount = data._cardCount;
        _recreateCount = data._recreateCount;

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
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateReusableCard, interacter.ReturnSkillUser(), _cardCount, _recreateCount);
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