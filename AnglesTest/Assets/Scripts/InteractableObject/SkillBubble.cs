using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillBubble : MonoBehaviour, IInteractable
{
    FollowComponent _followComponent;
    int _cardCount;
    float _moveSpeed;

    public void Initialize(SkillBubbleData data) 
    {
        _cardCount = data.CardCount;
        _moveSpeed = data.MoveSpeed;

        _followComponent = GetComponent<FollowComponent>();
        _followComponent.Initialize(_moveSpeed);
    }

    public void OnInteractEnter(IInteracter interacter)
    {
        IFollowable followable = interacter.ReturnFollower();
        _followComponent.InjectFollower(followable);
    }

    public void OnInteract(IInteracter interacter) 
    {
        EventBusManager.Instance.SubEventBus.Publish(SubEventBus.State.CreateCard, interacter.GetCaster(), _cardCount, 1);
        Destroy(gameObject);
    }

    private void Update()
    {
        _followComponent.OnUpdate();
    }

    private void FixedUpdate()
    {
        _followComponent.OnFixedUpdate();
    }

    public void OnInteractExit(IInteracter interacter) { }

    public void ResetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    GameObject IInteractable.ReturnGameObject() { return gameObject; }
}
