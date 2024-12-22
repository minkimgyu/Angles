using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class SkillBubbleData : BaseInteractableObjectData
{
    [JsonProperty] private int _cardCount;
    [JsonProperty] private float _moveSpeed;

    [JsonIgnore] public int CardCount { get => _cardCount; }
    [JsonIgnore] public float MoveSpeed { get => _moveSpeed; }

    public SkillBubbleData(int cardCount, float moveSpeed)
    {
        _cardCount = cardCount;
        _moveSpeed = moveSpeed;
    }
}

public class SkillBubbleCreater : InteractableObjectCreater
{
    //Action<List<SkillUpgradeData>> CreateCardsCommand;

    public SkillBubbleCreater(IInteractable interactablePrefab, BaseInteractableObjectData interactableData) 
        : base(interactablePrefab, interactableData)
    {
        //this.CreateCardsCommand = CreateCardsCommand;
    }

    public override IInteractable Create()
    {
        GameObject interactableObject = UnityEngine.Object.Instantiate(_interactablePrefab.ReturnGameObject());
        IInteractable interactable = interactableObject.GetComponent<IInteractable>();
        if (interactable == null) return null;

        SkillBubbleData data = _interactableData as SkillBubbleData;
        interactable.Initialize(data);
        return interactable;
    }
}