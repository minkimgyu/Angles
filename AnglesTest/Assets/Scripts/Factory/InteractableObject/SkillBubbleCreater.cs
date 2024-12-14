using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillBubbleData : BaseInteractableObjectData
{
    public int _cardCount;
    public float _moveSpeed;

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