using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillBubble : MonoBehaviour, IInteractable
{
    FollowComponent _trackComponent;
    int _cardCount;
    float _moveSpeed;

    public void Initialize(SkillBubbleData data) 
    {
        _cardCount = data.CardCount;
        _moveSpeed = data.MoveSpeed;

        _trackComponent = GetComponent<FollowComponent>();
        _trackComponent.Initialize(_moveSpeed);
    }

    public void OnInteractEnter(IInteracter interacter)
    {
        IFollowable followable = interacter.ReturnFollower();
        _trackComponent.ResetFollower(followable);
    }

    public void OnInteract(IInteracter interacter) 
    {
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateCard, interacter.GetCaster(), _cardCount);
        Destroy(gameObject);
    }

    public void OnInteractExit(IInteracter interacter) { }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
